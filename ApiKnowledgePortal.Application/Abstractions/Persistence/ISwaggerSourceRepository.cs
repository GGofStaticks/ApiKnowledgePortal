using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiKnowledgePortal.Domain.SwaggerSources;

namespace ApiKnowledgePortal.Application.Abstractions.Persistence
{
    public interface ISwaggerSourceRepository
    {
        Task<SwaggerSource?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IEnumerable<SwaggerSource>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<SwaggerSource>> GetAllActiveAsync(CancellationToken cancellationToken = default);
        Task AddAsync(SwaggerSource source, CancellationToken cancellationToken = default);
        Task UpdateAsync(SwaggerSource source, CancellationToken cancellationToken = default);
        Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    }
}