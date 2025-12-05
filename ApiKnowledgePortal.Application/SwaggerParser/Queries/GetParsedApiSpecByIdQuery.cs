using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiKnowledgePortal.Application.SwaggerParser.Dtos;
using MediatR;

namespace ApiKnowledgePortal.Application.SwaggerParser.Queries
{
    public record GetParsedApiSpecByIdQuery(Guid Id) : IRequest<ParsedApiSpecDto>;
}
