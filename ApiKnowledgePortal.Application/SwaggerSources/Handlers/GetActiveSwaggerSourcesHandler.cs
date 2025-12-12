using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiKnowledgePortal.Application.Abstractions.Persistence;
using ApiKnowledgePortal.Application.SwaggerSources.Dtos;
using ApiKnowledgePortal.Application.SwaggerSources.Mapping;
using ApiKnowledgePortal.Application.SwaggerSources.Queries;
using MediatR;

namespace ApiKnowledgePortal.Application.SwaggerSources.Handlers
{
    public class GetActiveSwaggerSourcesHandler : IRequestHandler<GetActiveSwaggerSourcesQuery, IEnumerable<SwaggerSourceDto>>
    {
        private readonly ISwaggerSourceRepository _repo;

        public GetActiveSwaggerSourcesHandler(ISwaggerSourceRepository repo) => _repo = repo;

        public async Task<IEnumerable<SwaggerSourceDto>> Handle(GetActiveSwaggerSourcesQuery request, CancellationToken ct)
        {
            var sources = await _repo.GetAllActiveAsync(ct);
            return sources.Select(s => s.ToDto());
        }
    }
}
