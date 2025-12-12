using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiKnowledgePortal.Application.Abstractions.Persistence;
using ApiKnowledgePortal.Application.Common;
using ApiKnowledgePortal.Application.SwaggerSources.Dtos;
using ApiKnowledgePortal.Application.SwaggerSources.Mapping;
using ApiKnowledgePortal.Application.SwaggerSources.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ApiKnowledgePortal.Application.SwaggerSources.Handlers
{
    public class GetSwaggerSourcesPagedHandler
        : IRequestHandler<GetSwaggerSourcesPagedQuery, PagedResult<SwaggerSourceDto>>
    {
        private readonly ISwaggerSourceRepository _repo;

        public GetSwaggerSourcesPagedHandler(ISwaggerSourceRepository repo) => _repo = repo;

        public async Task<PagedResult<SwaggerSourceDto>> Handle(GetSwaggerSourcesPagedQuery request, CancellationToken ct)
        {
            var page = Math.Max(request.Page, 1);
            var pageSize = Math.Max(request.PageSize, 1);
            var query = _repo.Query();
            var total = await query.CountAsync(ct);
            var items = await query
                .OrderBy(x => x.Name)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(ct);

            return new PagedResult<SwaggerSourceDto>
            {
                Items = items.Select(x => x.ToDto()),
                TotalCount = total,
                Page = page,
                PageSize = pageSize
            };
        }
    }
}
