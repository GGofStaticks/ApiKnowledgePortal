using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiKnowledgePortal.Application.Abstractions.Persistence;
using ApiKnowledgePortal.Application.SwaggerParser.Dtos;
using ApiKnowledgePortal.Application.SwaggerParser.Queries;
using AutoMapper;
using MediatR;

namespace ApiKnowledgePortal.Application.SwaggerParser.Handlers
{
    public class GetParsedApiSpecByIdHandler : IRequestHandler<GetParsedApiSpecByIdQuery, ParsedApiSpecDto>
    {
        private readonly IParsedApiSpecRepository _repository;
        private readonly IMapper _mapper;

        public GetParsedApiSpecByIdHandler(IParsedApiSpecRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ParsedApiSpecDto> Handle(GetParsedApiSpecByIdQuery request, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);

            if (entity is null)
                throw new KeyNotFoundException("спаршенная апи спецификация не найдена");

            return _mapper.Map<ParsedApiSpecDto>(entity);
        }
    }
}