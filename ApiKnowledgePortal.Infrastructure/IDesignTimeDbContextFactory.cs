using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiKnowledgePortal.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
// этот класс нужен, чтобы миграции не падали из за сборку диая
namespace ApiKnowledgePortal.Infrastructure
{
    public class ApiKnowledgePortalDbContextFactory : IDesignTimeDbContextFactory<ApiKnowledgePortalDbContext>
    {
        public ApiKnowledgePortalDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApiKnowledgePortalDbContext>();
            optionsBuilder.UseNpgsql("Host=localhost;Port=5433;Database=ApiKnowledgePortal;Username=postgres;Password=Gjtrnfert23");

            return new ApiKnowledgePortalDbContext(optionsBuilder.Options);
        }
    }
}
