using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiKnowledgePortal.Application.Abstractions.Persistence;
using ApiKnowledgePortal.Application.SwaggerParser.Commands;
using ApiKnowledgePortal.Application.SwaggerParser.Dtos;
using AutoMapper;
using MediatR;

namespace ApiKnowledgePortal.Application.SwaggerParser.Handlers
{
    public class UpdateParsedApiSpecHandler : IRequestHandler<UpdateParsedApiSpecCommand, ParsedApiSpecDto>
    {
        private readonly IParsedApiSpecRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateParsedApiSpecHandler(IParsedApiSpecRepository repository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ParsedApiSpecDto> Handle(UpdateParsedApiSpecCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);

            if (entity is null)
                throw new KeyNotFoundException("спаршенная апи спецификация не найдена");

            entity.UpdateSummary(request.Summary);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return _mapper.Map<ParsedApiSpecDto>(entity);
        }
    }
}
