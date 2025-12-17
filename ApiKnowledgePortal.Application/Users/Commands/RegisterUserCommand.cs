using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiKnowledgePortal.Application.Users.Dtos;
using MediatR;

namespace ApiKnowledgePortal.Application.Users.Commands
{
    public record RegisterUserCommand(string FirstName, string LastName, string Email, string Password) : IRequest<UserDto>;
}