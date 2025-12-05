using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiKnowledgePortal.Application.Abstractions.Persistence;
using ApiKnowledgePortal.Application.SwaggerSources.Commands;
using MediatR;

namespace ApiKnowledgePortal.Application.SwaggerSources.Handlers
{
    public class DeleteSwaggerSourceHandler : IRequestHandler<DeleteSwaggerSourceCommand, Unit>
    {
        private readonly ISwaggerSourceRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteSwaggerSourceHandler(ISwaggerSourceRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(DeleteSwaggerSourceCommand request, CancellationToken cancellationToken)
        {
            await _repository.DeleteAsync(request.Id, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}