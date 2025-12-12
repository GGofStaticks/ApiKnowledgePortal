using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiKnowledgePortal.Application.Abstractions.Persistence;
using ApiKnowledgePortal.Application.ApiSpec.Dtos;
using ApiKnowledgePortal.Application.ApiSpec.Queries;
using ApiKnowledgePortal.Application.Common;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ApiKnowledgePortal.Application.ApiSpec.Handlers
{
    public class GetApiSpecPagedHandler
        : IRequestHandler<GetApiSpecPagedQuery, PagedResult<ApiSpecDto>>
    {
        private readonly IApiSpecRepository _repo;
        private readonly IMapper _mapper;

        public GetApiSpecPagedHandler(IApiSpecRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<PagedResult<ApiSpecDto>> Handle(GetApiSpecPagedQuery request, CancellationToken ct)
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

            return new PagedResult<ApiSpecDto>
            {
                Items = _mapper.Map<IEnumerable<ApiSpecDto>>(items),
                TotalCount = total,
                Page = page,
                PageSize = pageSize
            };
        }
    }
}
