using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ApiKnowledgePortal.Application.Abstractions.Persistence;
using ApiKnowledgePortal.Application.ApiSpec.Dtos;
using ApiKnowledgePortal.Application.ApiSpec.Queries;
using ApiKnowledgePortal.Domain.Users;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace ApiKnowledgePortal.Application.ApiSpec.Handlers
{
    public class GetAllApiSpecsHandler : IRequestHandler<GetAllApiSpecsQuery, IEnumerable<ApiSpecDto>>
    {
        private readonly IApiSpecRepository _repository;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepo;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetAllApiSpecsHandler(IApiSpecRepository repository, IMapper mapper
            , IUserRepository userRepo
            , IHttpContextAccessor httpContextAccessor
        )
        {
            _repository = repository;
            _mapper = mapper;
            _userRepo = userRepo;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IEnumerable<ApiSpecDto>> Handle(GetAllApiSpecsQuery request, CancellationToken cancellationToken)
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
                        query = query.Where(a => a.SwaggerSourceId != null && user.Sources.Contains(a.SwaggerSourceId.Value));
                    }
                    else
                    {
                        query = query.Where(a => false);
                    }
                }
            }

            var list = await query.ToListAsync(cancellationToken);
            return _mapper.Map<IEnumerable<ApiSpecDto>>(list);
        }
    }
}
