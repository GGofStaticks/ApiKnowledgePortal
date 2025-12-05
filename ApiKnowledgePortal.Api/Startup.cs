using ApiKnowledgePortal.Application;
using ApiKnowledgePortal.Application.SwaggerParser.Service;
using ApiKnowledgePortal.Infrastructure;
using ApiKnowledgePortal.Infrastructure.Persistence;
using ApiKnowledgePortal.SyncWorker.Jobs;
using ApiKnowledgePortal.SyncWorker.Services;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.EntityFrameworkCore;

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
            app.UseAuthorization();
            app.UseEndpoints(endpoints => endpoints.MapControllers());

            app.UseHangfireDashboard();

            // запуск джоба на ежедневный запуск
            RecurringJob.AddOrUpdate<SwaggerFetcherJob>(
                job => job.ExecuteAsync(),
                Cron.Daily); // запуск один раз в день
        }
    }
}
