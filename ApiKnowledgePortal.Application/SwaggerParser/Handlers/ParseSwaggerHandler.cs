using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiKnowledgePortal.Application.Abstractions.Persistence;
using ApiKnowledgePortal.Application.SwaggerParser.Commands;
using ApiKnowledgePortal.Application.SwaggerParser.Service;
using MediatR;

namespace ApiKnowledgePortal.Application.SwaggerParser.Handlers
{
    public class ParseSwaggerHandler : IRequestHandler<ParseSwaggerCommand, Unit>
    {
        private readonly IApiSpecRepository _apiSpecRepo;
        private readonly SwaggerParserService _parser;

        public ParseSwaggerHandler(IApiSpecRepository apiSpecRepo, SwaggerParserService parser)
        {
            _apiSpecRepo = apiSpecRepo;
            _parser = parser;
        }

        public async Task<Unit> Handle(ParseSwaggerCommand request, CancellationToken cancellationToken)
        {
            var spec = await _apiSpecRepo.GetByIdAsync(new ApiKnowledgePortal.Domain.ValueObjects.ApiSpecId(request.ApiSpecId));
            if (spec != null)
            {
                await _parser.ParseAndSaveAsync(spec);
            }
            return Unit.Value;
        }
    }
}