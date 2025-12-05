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
    public class GetAllParsedApiSpecsHandler : IRequestHandler<GetAllParsedApiSpecsQuery, IEnumerable<ParsedApiSpecDto>>
    {
        private readonly IParsedApiSpecRepository _repository;
        private readonly IMapper _mapper;

        public GetAllParsedApiSpecsHandler(IParsedApiSpecRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ParsedApiSpecDto>> Handle(GetAllParsedApiSpecsQuery request, CancellationToken cancellationToken)
        {
            var list = await _repository.GetAllAsync(cancellationToken);
            return _mapper.Map<IEnumerable<ParsedApiSpecDto>>(list);
        }
    }
}
