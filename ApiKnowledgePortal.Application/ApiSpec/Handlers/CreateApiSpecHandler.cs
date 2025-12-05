using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiKnowledgePortal.Application.Abstractions.Persistence;
using ApiKnowledgePortal.Application.ApiSpec.Commands;
using ApiKnowledgePortal.Application.ApiSpec.Dtos;
using ApiKnowledgePortal.Domain.ApiSpecifications;
using ApiKnowledgePortal.Domain.ValueObjects;
using AutoMapper;
using MediatR;

namespace ApiKnowledgePortal.Application.ApiSpec.Handlers
{
    public class CreateApiSpecHandler : IRequestHandler<CreateApiSpecCommand, ApiSpecDto>
    {
        private readonly IApiSpecRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateApiSpecHandler(IApiSpecRepository repository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ApiSpecDto> Handle(CreateApiSpecCommand request, CancellationToken cancellationToken)
        {
            var id = ApiSpecId.NewId();
            var spec = new ApiSpecifications(id, request.Name, request.Version, request.Content);

            await _repository.AddAsync(spec, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return _mapper.Map<ApiSpecDto>(spec);
        }
    }
}