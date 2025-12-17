using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace ApiKnowledgePortal.Application.Users.Commands
{
    public record DeleteUserCommand(Guid Id) : IRequest<Unit>;
}
