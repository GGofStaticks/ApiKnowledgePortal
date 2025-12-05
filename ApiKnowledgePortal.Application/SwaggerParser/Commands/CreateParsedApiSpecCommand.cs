using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiKnowledgePortal.Application.SwaggerParser.Dtos;
using MediatR;

namespace ApiKnowledgePortal.Application.SwaggerParser.Commands
{
    public record CreateParsedApiSpecCommand(Guid ApiSpecId, string Path, string Method, string OperationId, string Summary) : IRequest<ParsedApiSpecDto>;
}