using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiKnowledgePortal.Application.Abstractions.Persistence;
using ApiKnowledgePortal.Application.SwaggerParser.Commands;
using ApiKnowledgePortal.Application.SwaggerParser.Dtos;
using ApiKnowledgePortal.Domain.ParsedApiSpecs;
using AutoMapper;
using MediatR;

namespace ApiKnowledgePortal.Application.SwaggerParser.Handlers
{
    public class CreateParsedApiSpecHandler : IRequestHandler<CreateParsedApiSpecCommand, ParsedApiSpecDto>
    {
        private readonly IParsedApiSpecRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateParsedApiSpecHandler(
            IParsedApiSpecRepository repository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ParsedApiSpecDto> Handle(CreateParsedApiSpecCommand request, CancellationToken cancellationToken)
        {
            // парседспек сам генерит айди в конструкторе
            var entity = new ParsedApiSpec(
                request.ApiSpecId,
                request.Path,
                request.Method,
                request.OperationId,
                request.Summary
            );

            await _repository.AddAsync(entity, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return _mapper.Map<ParsedApiSpecDto>(entity);
        }
    }
}