using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiKnowledgePortal.Application.Abstractions.Persistence;
using ApiKnowledgePortal.Application.ApiSpec.Dtos;
using ApiKnowledgePortal.Application.ApiSpec.Queries;
using ApiKnowledgePortal.Domain.ValueObjects;
using AutoMapper;
using MediatR;

namespace ApiKnowledgePortal.Application.ApiSpec.Handlers
{
    public class GetApiSpecByIdHandler : IRequestHandler<GetApiSpecByIdQuery, ApiSpecDto>
    {
        private readonly IApiSpecRepository _repository;
        private readonly IMapper _mapper;

        public GetApiSpecByIdHandler(IApiSpecRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ApiSpecDto> Handle(GetApiSpecByIdQuery request, CancellationToken cancellationToken)
        {
            var id = new ApiSpecId(request.Id);
            var spec = await _repository.GetByIdAsync(id, cancellationToken);
            if (spec == null) return null!;
            return _mapper.Map<ApiSpecDto>(spec);
        }
    }
}