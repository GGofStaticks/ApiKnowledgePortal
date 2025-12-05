using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace ApiKnowledgePortal.Application.SwaggerSources.Commands
{
    public record DeleteSwaggerSourceCommand(Guid Id) : IRequest<Unit>;
}