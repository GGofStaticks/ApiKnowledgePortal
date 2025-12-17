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
using ApiKnowledgePortal.Domain.ValueObjects;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace ApiKnowledgePortal.Application.ApiSpec.Handlers
{
    public class GetApiSpecByIdHandler : IRequestHandler<GetApiSpecByIdQuery, ApiSpecDto>
    {
        private readonly IApiSpecRepository _repository;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepo;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetApiSpecByIdHandler(IApiSpecRepository repository, IMapper mapper
            , IUserRepository userRepo
            , IHttpContextAccessor httpContextAccessor
        )
        {
            _repository = repository;
            _mapper = mapper;
            _userRepo = userRepo;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ApiSpecDto> Handle(GetApiSpecByIdQuery request, CancellationToken cancellationToken)
        {
            var id = new ApiSpecId(request.Id);
            var spec = await _repository.GetByIdAsync(id, cancellationToken);
            if (spec == null) return null!;

            var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim != null && Guid.TryParse(userIdClaim.Value, out var userId))
            {
                var user = await _userRepo.GetByIdAsync(userId, cancellationToken);
                if (user != null && user.Role != UserRole.Admin &&
                    (spec.SwaggerSourceId == null || !user.Sources.Contains(spec.SwaggerSourceId.Value)))
                {
                    throw new UnauthorizedAccessException("Доступ запрещён: спецификация не принадлежит вашему источнику");
                }
            }

            return _mapper.Map<ApiSpecDto>(spec);
        }
    }
}