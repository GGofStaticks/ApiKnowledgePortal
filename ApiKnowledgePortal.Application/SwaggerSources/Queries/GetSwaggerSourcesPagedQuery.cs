using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiKnowledgePortal.Application.Common;
using ApiKnowledgePortal.Application.SwaggerSources.Dtos;
using MediatR;

namespace ApiKnowledgePortal.Application.SwaggerSources.Queries
{
    public record GetSwaggerSourcesPagedQuery(int Page = 1, int PageSize  = 10): IRequest<PagedResult<SwaggerSourceDto>>;
}
