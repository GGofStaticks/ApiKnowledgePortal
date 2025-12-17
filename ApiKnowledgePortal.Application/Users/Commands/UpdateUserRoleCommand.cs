using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiKnowledgePortal.Application.Users.Dtos;
using ApiKnowledgePortal.Domain.Users;
using MediatR;

namespace ApiKnowledgePortal.Application.Users.Commands
{
    public record UpdateUserRoleCommand(Guid UserId, UserRole NewRole) : IRequest<UserDto>;
}
