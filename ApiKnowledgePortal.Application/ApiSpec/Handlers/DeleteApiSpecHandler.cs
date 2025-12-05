using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiKnowledgePortal.Application.Abstractions.Persistence;
using ApiKnowledgePortal.Application.ApiSpec.Commands;
using ApiKnowledgePortal.Domain.ValueObjects;
using MediatR;

namespace ApiKnowledgePortal.Application.ApiSpec.Handlers
{
    public class DeleteApiSpecHandler : IRequestHandler<DeleteApiSpecCommand, Unit>
    {
        private readonly IApiSpecRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteApiSpecHandler(IApiSpecRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(DeleteApiSpecCommand request, CancellationToken cancellationToken)
        {
            var id = new ApiSpecId(request.Id);
            await _repository.DeleteAsync(id, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}