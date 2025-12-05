using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiKnowledgePortal.Application.Abstractions.Persistence;
using ApiKnowledgePortal.Domain.ApiSpecifications;
using ApiKnowledgePortal.Domain.ValueObjects;
using ApiKnowledgePortal.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ApiKnowledgePortal.Infrastructure.Repositories
{
    public class ApiSpecRepository : IApiSpecRepository
    {
        private readonly ApiKnowledgePortalDbContext _dbContext;

        public ApiSpecRepository(ApiKnowledgePortalDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(ApiSpecifications apiSpec, CancellationToken cancellationToken = default)
        {
            await _dbContext.ApiSpecs.AddAsync(apiSpec, cancellationToken);
        }

        public async Task DeleteAsync(ApiSpecId id, CancellationToken cancellationToken = default)
        {
            var entity = await _dbContext.ApiSpecs.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
            if (entity != null)
            {
                _dbContext.ApiSpecs.Remove(entity);
            }
        }

        public async Task<IEnumerable<ApiSpecifications>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.ApiSpecs.ToListAsync(cancellationToken);
        }

        public async Task<ApiSpecifications?> GetByIdAsync(ApiSpecId id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.ApiSpecs.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task UpdateAsync(ApiSpecifications apiSpec, CancellationToken cancellationToken = default)
        {
            _dbContext.ApiSpecs.Update(apiSpec);
            await Task.CompletedTask;
        }
    }
}