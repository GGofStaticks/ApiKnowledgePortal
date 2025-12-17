using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ApiKnowledgePortal.Application.Abstractions.Persistence;
using ApiKnowledgePortal.Application.Common;
using ApiKnowledgePortal.Application.SwaggerParser.Dtos;
using ApiKnowledgePortal.Application.SwaggerParser.Queries;
using ApiKnowledgePortal.Domain.Users;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace ApiKnowledgePortal.Application.SwaggerParser.Handlers
{
    public class GetParsedApiSpecsPagedHandler
        : IRequestHandler<GetParsedApiSpecsPagedQuery, PagedResult<ParsedApiSpecDto>>
    {
        private readonly IParsedApiSpecRepository _repo;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepo;
        private readonly IApiSpecRepository _apiSpecRepo;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetParsedApiSpecsPagedHandler(
            IParsedApiSpecRepository repo,
            IMapper mapper
            , IUserRepository userRepo
            , IApiSpecRepository apiSpecRepo
            , IHttpContextAccessor httpContextAccessor
        )
        {
            _repo = repo;
            _mapper = mapper;
            _userRepo = userRepo;
            _apiSpecRepo = apiSpecRepo;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<PagedResult<ParsedApiSpecDto>> Handle(
            GetParsedApiSpecsPagedQuery request,
            CancellationToken cancellationToken)
        {
            var page = Math.Max(request.Page, 1);
            var pageSize = Math.Max(request.PageSize, 1);
            var query = _repo.Query();

            // фильтрация по источника пользователя через связь с аписпексами
            var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim != null && Guid.TryParse(userIdClaim.Value, out var userId))
            {
                var user = await _userRepo.GetByIdAsync(userId, cancellationToken);
                if (user != null && user.Role != UserRole.Admin)
                {
                    if (!user.Sources.Any())
                    {
                        query = query.Where(p => false); // нет источников, значит ничего не показывать
                    }
                    else
                    {
                        // все аписпекс принадлежащие источникам пользователя
                        var allowedApiSpecIds = await _apiSpecRepo.Query()
                            .Where(a => a.SwaggerSourceId != null && user.Sources.Contains(a.SwaggerSourceId.Value))
                            .Select(a => a.Id.Value)
                            .ToListAsync(cancellationToken);

                        query = query.Where(p => allowedApiSpecIds.Contains(p.ApiSpecId));
                    }
                }
            }

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
