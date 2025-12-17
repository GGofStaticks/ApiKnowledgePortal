using ApiKnowledgePortal.Application;
using ApiKnowledgePortal.Application.SwaggerParser.Service;
using ApiKnowledgePortal.Infrastructure;
using ApiKnowledgePortal.Infrastructure.Persistence;
using ApiKnowledgePortal.SyncWorker.Jobs;
using ApiKnowledgePortal.SyncWorker.Services;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace ApiKnowledgePortal.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            Environment = env;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddApplication();
            services.AddInfrastructure(Configuration);
            services.AddHttpClient();

            // свагерфетчер
            services.AddScoped<SwaggerFetcherService>();
            services.AddScoped<SwaggerFetcherJob>();
            services.AddScoped<SwaggerParserService>();

            services.AddDbContext<ApiKnowledgePortalDbContext>(options =>
            options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));

            // хенгфаер
            services.AddHangfire(config =>
                config.UsePostgreSqlStorage(Configuration.GetConnectionString("DefaultConnection")));
            services.AddHangfireServer();

            services.AddCors(options =>
            {
                options.AddPolicy("AllowLocalhost", policy =>
                {
                    policy.WithOrigins("https://localhost:7236")
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });

            // HttpContextAccessor для доступа к claims в handlers
            services.AddHttpContextAccessor();

            // JWT
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = Configuration["Jwt:Issuer"],
                        ValidAudience = Configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
                    };
                });

            services.AddAuthorization();
        }

        public void Configure(IApplicationBuilder app)
        {
            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseAuthorization();

            app.UseCors("AllowLocalhost");

            app.UseEndpoints(endpoints => endpoints.MapControllers());

            app.UseHangfireDashboard();

            // запуск джоба на ежедневный запуск
            RecurringJob.AddOrUpdate<SwaggerFetcherJob>(
                job => job.ExecuteAsync(),
                Cron.Daily); // запуск один раз в день
        }
    }
}
