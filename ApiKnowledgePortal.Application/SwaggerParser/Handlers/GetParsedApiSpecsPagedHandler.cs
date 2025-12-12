using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiKnowledgePortal.Application.Abstractions.Persistence;
using ApiKnowledgePortal.Application.Common;
using ApiKnowledgePortal.Application.SwaggerParser.Dtos;
using ApiKnowledgePortal.Application.SwaggerParser.Queries;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ApiKnowledgePortal.Application.SwaggerParser.Handlers
{
    public class GetParsedApiSpecsPagedHandler
        : IRequestHandler<GetParsedApiSpecsPagedQuery, PagedResult<ParsedApiSpecDto>>
    {
        private readonly IParsedApiSpecRepository _repo;
        private readonly IMapper _mapper;

        public GetParsedApiSpecsPagedHandler(IParsedApiSpecRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<PagedResult<ParsedApiSpecDto>> Handle(
            GetParsedApiSpecsPagedQuery request,
            CancellationToken cancellationToken)
        {
            var page = Math.Max(request.Page, 1);
            var pageSize = Math.Max(request.PageSize, 1);

            var query = _repo.Query();

            var total = await query.CountAsync(cancellationToken);

            var items = await query
                .OrderBy(x => x.Path)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return new PagedResult<ParsedApiSpecDto>
            {
                Items = _mapper.Map<IEnumerable<ParsedApiSpecDto>>(items),
                TotalCount = total,
                Page = page,
                PageSize = pageSize
            };
        }
    }
}
