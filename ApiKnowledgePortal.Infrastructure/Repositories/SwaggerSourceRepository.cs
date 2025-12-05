using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiKnowledgePortal.Application.Abstractions.Persistence;
using ApiKnowledgePortal.Domain.SwaggerSources;
using ApiKnowledgePortal.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ApiKnowledgePortal.Infrastructure.Repositories
{
    public class SwaggerSourceRepository : ISwaggerSourceRepository
    {
        private readonly ApiKnowledgePortalDbContext _dbContext;

        public SwaggerSourceRepository(ApiKnowledgePortalDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(SwaggerSource source, CancellationToken cancellationToken = default)
        {
            await _dbContext.SwaggerSources.AddAsync(source, cancellationToken);
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var entity = await _dbContext.SwaggerSources.FindAsync(new object[] { id }, cancellationToken);
            if (entity != null)
            {
                _dbContext.SwaggerSources.Remove(entity);
            }
        }

        public async Task<IEnumerable<SwaggerSource>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.SwaggerSources.ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<SwaggerSource>> GetAllActiveAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.SwaggerSources
                .Where(s => s.IsActive)
                .ToListAsync(cancellationToken);
        }

        public async Task<SwaggerSource?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.SwaggerSources.FindAsync(new object[] { id }, cancellationToken);
        }

        public async Task UpdateAsync(SwaggerSource source, CancellationToken cancellationToken = default)
        {
            _dbContext.SwaggerSources.Update(source);
            await Task.CompletedTask;
        }
    }
}