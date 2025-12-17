using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ApiKnowledgePortal.Application.Abstractions.Persistence;
using ApiKnowledgePortal.Application.Common;
using ApiKnowledgePortal.Application.SwaggerSources.Dtos;
using ApiKnowledgePortal.Application.SwaggerSources.Mapping;
using ApiKnowledgePortal.Application.SwaggerSources.Queries;
using ApiKnowledgePortal.Domain.Users;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace ApiKnowledgePortal.Application.SwaggerSources.Handlers
{
    public class GetSwaggerSourcesPagedHandler
        : IRequestHandler<GetSwaggerSourcesPagedQuery, PagedResult<SwaggerSourceDto>>
    {
        private readonly ISwaggerSourceRepository _repo;

        private readonly IUserRepository _userRepo;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetSwaggerSourcesPagedHandler(ISwaggerSourceRepository repo, IUserRepository userRepo, IHttpContextAccessor httpContextAccessor)
        {
            _repo = repo;
            _userRepo = userRepo;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<PagedResult<SwaggerSourceDto>> Handle(GetSwaggerSourcesPagedQuery request, CancellationToken ct)
        {
            var page = Math.Max(request.Page, 1);
            var pageSize = Math.Max(request.PageSize, 1);
            var query = _repo.Query();

            // фильтрация по ролям и источникам
            var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim != null && Guid.TryParse(userIdClaim.Value, out var userId))
            {
                var user = await _userRepo.GetByIdAsync(userId, ct);
                if (user != null && user.Role != UserRole.Admin)
                {
                    query = query.Where(s => user.Sources.Contains(s.Id));
                }
            }

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
