using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace ApiKnowledgePortal.Application.SwaggerParser.Commands
{
    public record DeleteParsedApiSpecCommand(Guid Id) : IRequest<Unit>;
}