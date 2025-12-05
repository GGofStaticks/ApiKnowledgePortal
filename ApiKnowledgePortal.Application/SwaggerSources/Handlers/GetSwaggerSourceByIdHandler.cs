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
    public class GetSwaggerSourceByIdHandler : IRequestHandler<GetSwaggerSourceByIdQuery, SwaggerSourceDto>
    {
        private readonly ISwaggerSourceRepository _repository;

        public GetSwaggerSourceByIdHandler(ISwaggerSourceRepository repository)
        {
            _repository = repository;
        }

        public async Task<SwaggerSourceDto> Handle(GetSwaggerSourceByIdQuery request, CancellationToken cancellationToken)
        {
            var source = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (source == null) throw new Exception("источник свагера не найден");
            return source.ToDto();
        }
    }
}
