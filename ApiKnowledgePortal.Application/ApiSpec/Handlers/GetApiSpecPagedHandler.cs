using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ApiKnowledgePortal.Application.Abstractions.Persistence;
using ApiKnowledgePortal.Application.ApiSpec.Dtos;
using ApiKnowledgePortal.Application.ApiSpec.Queries;
using ApiKnowledgePortal.Application.Common;
using ApiKnowledgePortal.Domain.Users;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace ApiKnowledgePortal.Application.ApiSpec.Handlers
{
    public class GetApiSpecPagedHandler
        : IRequestHandler<GetApiSpecPagedQuery, PagedResult<ApiSpecDto>>
    {
        private readonly IApiSpecRepository _repo;
        private readonly IMapper _mapper;

        private readonly IUserRepository _userRepo;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetApiSpecPagedHandler(IApiSpecRepository repo, IMapper mapper, IUserRepository userRepo, IHttpContextAccessor httpContextAccessor)
        {
            _repo = repo;
            _mapper = mapper;
            _userRepo = userRepo;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<PagedResult<ApiSpecDto>> Handle(GetApiSpecPagedQuery request, CancellationToken ct)
        {
            var page = Math.Max(request.Page, 1);
            var pageSize = Math.Max(request.PageSize, 1);
            var query = _repo.Query();

            // фильтрация аписпкесов по источникам пользователя, если он не админ
            var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim != null && Guid.TryParse(userIdClaim.Value, out var userId))
            {
                var user = await _userRepo.GetByIdAsync(userId, ct);
                if (user != null && user.Role != UserRole.Admin)
                {
                    query = query.Where(a => user.Sources.Contains(a.SwaggerSourceId ?? Guid.Empty));
                }
            }

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
