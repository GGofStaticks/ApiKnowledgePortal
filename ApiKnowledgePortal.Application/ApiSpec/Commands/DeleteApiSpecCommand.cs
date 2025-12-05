using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace ApiKnowledgePortal.Application.ApiSpec.Commands
{
    public record DeleteApiSpecCommand(Guid Id) : IRequest<Unit>;
}
