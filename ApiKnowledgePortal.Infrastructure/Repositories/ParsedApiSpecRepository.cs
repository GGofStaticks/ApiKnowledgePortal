using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiKnowledgePortal.Application.Abstractions.Persistence;
using ApiKnowledgePortal.Domain.ParsedApiSpecs;
using ApiKnowledgePortal.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ApiKnowledgePortal.Infrastructure.Repositories
{
    public class ParsedApiSpecRepository : IParsedApiSpecRepository
    {
        private readonly ApiKnowledgePortalDbContext _dbContext;
        public ParsedApiSpecRepository(ApiKnowledgePortalDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(ParsedApiSpec parsed, CancellationToken cancellationToken = default)
        {
            await _dbContext.ParsedApiSpecs.AddAsync(parsed, cancellationToken);
        }

        public async Task<IEnumerable<ParsedApiSpec>> GetByApiSpecIdAsync(Guid apiSpecId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.ParsedApiSpecs
                .Where(p => p.ApiSpecId == apiSpecId)
                .ToListAsync(cancellationToken);
        }

        public async Task<ParsedApiSpec?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.ParsedApiSpecs
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public Task UpdateAsync(ParsedApiSpec parsed, CancellationToken cancellationToken = default)
        {
            _dbContext.ParsedApiSpecs.Update(parsed);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(ParsedApiSpec parsed, CancellationToken cancellationToken = default)
        {
            _dbContext.ParsedApiSpecs.Remove(parsed);
            return Task.CompletedTask;
        }
        public async Task<IEnumerable<ParsedApiSpec>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.ParsedApiSpecs.ToListAsync(cancellationToken);
        }
        public IQueryable<ParsedApiSpec> Query() => _dbContext.ParsedApiSpecs.AsQueryable();
    }
}