using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiKnowledgePortal.Domain.ParsedApiSpecs;

namespace ApiKnowledgePortal.Application.Abstractions.Persistence
{
    public interface IParsedApiSpecRepository
    {
        Task AddAsync(ParsedApiSpec parsed, CancellationToken cancellationToken = default);
        Task<IEnumerable<ParsedApiSpec>> GetByApiSpecIdAsync(Guid apiSpecId, CancellationToken cancellationToken = default);
        Task<ParsedApiSpec?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task UpdateAsync(ParsedApiSpec parsed, CancellationToken cancellationToken = default);
        Task DeleteAsync(ParsedApiSpec parsed, CancellationToken cancellationToken = default);
        Task<IEnumerable<ParsedApiSpec>> GetAllAsync(CancellationToken cancellationToken = default);

    }
}