using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiKnowledgePortal.Application.Abstractions.Persistence;
using ApiKnowledgePortal.Application.SwaggerSources.Commands;
using ApiKnowledgePortal.Application.SwaggerSources.Dtos;
using ApiKnowledgePortal.Application.SwaggerSources.Mapping;
using MediatR;

namespace ApiKnowledgePortal.Application.SwaggerSources.Handlers
{
    public class UpdateSwaggerSourceHandler : IRequestHandler<UpdateSwaggerSourceCommand, SwaggerSourceDto>
    {
        private readonly ISwaggerSourceRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateSwaggerSourceHandler(ISwaggerSourceRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<SwaggerSourceDto> Handle(UpdateSwaggerSourceCommand request, CancellationToken cancellationToken)
        {
            var source = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (source == null) throw new Exception("источник свагера не найден");

            source.UpdateName(request.Name);
            source.UpdateUrl(request.Url);
            if (request.IsActive) source.Activate(); else source.Deactivate();

            await _repository.UpdateAsync(source, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return source.ToDto();
        }
    }
}