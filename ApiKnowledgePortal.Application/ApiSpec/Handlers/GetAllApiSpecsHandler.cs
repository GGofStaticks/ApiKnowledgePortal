using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiKnowledgePortal.Application.Abstractions.Persistence;
using ApiKnowledgePortal.Application.ApiSpec.Dtos;
using ApiKnowledgePortal.Application.ApiSpec.Queries;
using AutoMapper;
using MediatR;

namespace ApiKnowledgePortal.Application.ApiSpec.Handlers
{
    public class GetAllApiSpecsHandler : IRequestHandler<GetAllApiSpecsQuery, IEnumerable<ApiSpecDto>>
    {
        private readonly IApiSpecRepository _repository;
        private readonly IMapper _mapper;

        public GetAllApiSpecsHandler(IApiSpecRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ApiSpecDto>> Handle(GetAllApiSpecsQuery request, CancellationToken cancellationToken)
        {
            var list = await _repository.GetAllAsync(cancellationToken);
            return _mapper.Map<IEnumerable<ApiSpecDto>>(list);
        }
    }
}
