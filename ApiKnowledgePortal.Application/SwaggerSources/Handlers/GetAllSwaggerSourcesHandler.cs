using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ApiKnowledgePortal.Application.Abstractions.Persistence;
using ApiKnowledgePortal.Application.SwaggerSources.Dtos;
using ApiKnowledgePortal.Application.SwaggerSources.Mapping;
using ApiKnowledgePortal.Application.SwaggerSources.Queries;
using ApiKnowledgePortal.Domain.Users;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace ApiKnowledgePortal.Application.SwaggerSources.Handlers
{
    public class GetAllSwaggerSourcesHandler : IRequestHandler<GetAllSwaggerSourcesQuery, IEnumerable<SwaggerSourceDto>>
    {
        private readonly ISwaggerSourceRepository _repository;
        private readonly IUserRepository _userRepo;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetAllSwaggerSourcesHandler(ISwaggerSourceRepository repository, IUserRepository userRepo, IHttpContextAccessor httpContextAccessor
        )
        {
            _repository = repository;
            _userRepo = userRepo;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IEnumerable<SwaggerSourceDto>> Handle(GetAllSwaggerSourcesQuery request, CancellationToken cancellationToken)
        {
            var query = _repository.Query();

            var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim != null && Guid.TryParse(userIdClaim.Value, out var userId))
            {
                var user = await _userRepo.GetByIdAsync(userId, cancellationToken);
                if (user != null && user.Role != UserRole.Admin)
                {
                    if (user.Sources.Any())
                    {
                        query = query.Where(s => user.Sources.Contains(s.Id));
                    }
                    else
                    {
                        query = query.Where(s => false);
                    }
                }
            }

            var sources = await query.ToListAsync(cancellationToken);
            return sources.Select(s => s.ToDto());
        }
    }
}