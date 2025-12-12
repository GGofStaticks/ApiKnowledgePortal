using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiKnowledgePortal.Application.ApiSpec.Dtos;
using ApiKnowledgePortal.Application.Common;
using MediatR;

namespace ApiKnowledgePortal.Application.ApiSpec.Queries
{
    public record GetApiSpecPagedQuery(int Page = 1, int PageSize = 10): IRequest<PagedResult<ApiSpecDto>>;
}
