using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiKnowledgePortal.Application.Abstractions.Persistence;
using ApiKnowledgePortal.Application.SwaggerSources.Commands;
using ApiKnowledgePortal.Application.SwaggerSources.Dtos;
using ApiKnowledgePortal.Application.SwaggerSources.Mapping;
using ApiKnowledgePortal.Domain.SwaggerSources;
using MediatR;

namespace ApiKnowledgePortal.Application.SwaggerSources.Handlers
{
    public class CreateSwaggerSourceHandler : IRequestHandler<CreateSwaggerSourceCommand, SwaggerSourceDto>
    {
        private readonly ISwaggerSourceRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateSwaggerSourceHandler(ISwaggerSourceRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<SwaggerSourceDto> Handle(CreateSwaggerSourceCommand request, CancellationToken cancellationToken)
        {
            var source = new SwaggerSource(Guid.NewGuid(), request.Name, request.Url);

            await _repository.AddAsync(source, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return source.ToDto();
        }
    }
}