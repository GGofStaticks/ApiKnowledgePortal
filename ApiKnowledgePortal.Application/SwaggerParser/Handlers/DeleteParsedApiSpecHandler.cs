using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiKnowledgePortal.Application.Abstractions.Persistence;
using ApiKnowledgePortal.Application.SwaggerParser.Commands;
using MediatR;

namespace ApiKnowledgePortal.Application.SwaggerParser.Handlers
{
    public class DeleteParsedApiSpecHandler : IRequestHandler<DeleteParsedApiSpecCommand, Unit>
    {
        private readonly IParsedApiSpecRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteParsedApiSpecHandler(IParsedApiSpecRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(DeleteParsedApiSpecCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);

            if (entity is null)
                throw new KeyNotFoundException("спаршенная апи спецификация не найдена");

            await _repository.DeleteAsync(entity);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
