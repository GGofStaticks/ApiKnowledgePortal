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
    public class GetAllSwaggerSourcesHandler : IRequestHandler<GetAllSwaggerSourcesQuery, IEnumerable<SwaggerSourceDto>>
    {
        private readonly ISwaggerSourceRepository _repository;

        public GetAllSwaggerSourcesHandler(ISwaggerSourceRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<SwaggerSourceDto>> Handle(GetAllSwaggerSourcesQuery request, CancellationToken cancellationToken)
        {
            var sources = await _repository.GetAllAsync(cancellationToken);
            return sources.Select(s => s.ToDto());
        }
    }
}