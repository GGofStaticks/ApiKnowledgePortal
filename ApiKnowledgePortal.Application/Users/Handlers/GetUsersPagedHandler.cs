using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiKnowledgePortal.Application.Abstractions.Persistence;
using ApiKnowledgePortal.Application.Common;
using ApiKnowledgePortal.Application.Users.Dtos;
using ApiKnowledgePortal.Application.Users.Queries;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ApiKnowledgePortal.Application.Users.Handlers
{
    public class GetUsersPagedHandler : IRequestHandler<GetUsersPagedQuery, PagedResult<UserDto>>
    {
        private readonly IUserRepository _repo;
        private readonly IMapper _mapper;

        public GetUsersPagedHandler(IUserRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<PagedResult<UserDto>> Handle(GetUsersPagedQuery request, CancellationToken ct)
        {
            var page = Math.Max(request.Page, 1);
            var pageSize = Math.Max(request.PageSize, 1);
            var query = _repo.Query();
            var total = await query.CountAsync(ct);
            var items = await query
                .OrderBy(x => x.Email)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(ct);
            return new PagedResult<UserDto>
            {
                Items = _mapper.Map<IEnumerable<UserDto>>(items),
                TotalCount = total,
                Page = page,
                PageSize = pageSize
            };
        }
    }
}
