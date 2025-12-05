using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiKnowledgePortal.Application.Abstractions.Persistence;
using ApiKnowledgePortal.Application.ApiSpec.Commands;
using ApiKnowledgePortal.Application.ApiSpec.Dtos;
using ApiKnowledgePortal.Domain.ValueObjects;
using AutoMapper;
using MediatR;

namespace ApiKnowledgePortal.Application.ApiSpec.Handlers
{
    public class UpdateApiSpecHandler : IRequestHandler<UpdateApiSpecCommand, ApiSpecDto>
    {
        private readonly IApiSpecRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateApiSpecHandler(IApiSpecRepository repository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ApiSpecDto> Handle(UpdateApiSpecCommand request, CancellationToken cancellationToken)
        {
            var id = new ApiSpecId(request.Id);
            var spec = await _repository.GetByIdAsync(id, cancellationToken);
            if (spec == null) throw new InvalidOperationException("спецификация не найдена");

            spec.UpdateContent(request.Content);
            await _repository.UpdateAsync(spec, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return _mapper.Map<ApiSpecDto>(spec);
        }
    }
}