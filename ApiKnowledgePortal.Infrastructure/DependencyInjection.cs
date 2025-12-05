using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiKnowledgePortal.Application.Abstractions.Persistence;
using ApiKnowledgePortal.Infrastructure.Persistence;
using ApiKnowledgePortal.Infrastructure.Repositories;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ApiKnowledgePortal.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // контекс бд для потсгреса
            services.AddDbContext<ApiKnowledgePortalDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

            // юнитофворк
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // репозитории
            services.AddScoped<IApiSpecRepository, ApiSpecRepository>();
            services.AddScoped<ISwaggerSourceRepository, SwaggerSourceRepository>();
            services.AddScoped<IParsedApiSpecRepository, ParsedApiSpecRepository>();

            // хенгфаер
            services.AddHangfire(config => config
                .UsePostgreSqlStorage(configuration.GetConnectionString("DefaultConnection")));
            services.AddHangfireServer();

            return services;
        }
    }
}