using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiKnowledgePortal.Domain.ApiSpecifications;
using ApiKnowledgePortal.Domain.ValueObjects;

namespace ApiKnowledgePortal.Application.Abstractions.Persistence
{
    public interface IApiSpecRepository
    {
        Task<ApiSpecifications?> GetByIdAsync(ApiSpecId id, CancellationToken cancellationToken = default);
        Task<IEnumerable<ApiSpecifications>> GetAllAsync(CancellationToken cancellationToken = default);
        Task AddAsync(ApiSpecifications apiSpec, CancellationToken cancellationToken = default);
        Task UpdateAsync(ApiSpecifications apiSpec, CancellationToken cancellationToken = default);
        Task DeleteAsync(ApiSpecId id, CancellationToken cancellationToken = default);
        IQueryable<ApiSpecifications> Query();
    }
}