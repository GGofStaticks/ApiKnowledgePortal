using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiKnowledgePortal.Application.Common;
using ApiKnowledgePortal.Application.Users.Dtos;
using MediatR;

namespace ApiKnowledgePortal.Application.Users.Queries
{
    public record GetUsersPagedQuery(int Page = 1, int PageSize = 10) : IRequest<PagedResult<UserDto>>;
}
