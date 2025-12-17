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

namespace ApiKnowledgePortal.Application.SwaggerSources.Handlers
{
    public class GetSwaggerSourceByIdHandler : IRequestHandler<GetSwaggerSourceByIdQuery, SwaggerSourceDto>
    {
        private readonly ISwaggerSourceRepository _repository;
        private readonly IUserRepository _userRepo;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetSwaggerSourceByIdHandler(ISwaggerSourceRepository repository, IUserRepository userRepo, IHttpContextAccessor httpContextAccessor
        )
        {
            _repository = repository;
            _userRepo = userRepo;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<SwaggerSourceDto> Handle(GetSwaggerSourceByIdQuery request, CancellationToken cancellationToken)
        {
            var source = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (source == null) throw new Exception("источник свагера не найден");

            var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim != null && Guid.TryParse(userIdClaim.Value, out var userId))
            {
                var user = await _userRepo.GetByIdAsync(userId, cancellationToken);
                if (user != null && user.Role != UserRole.Admin && !user.Sources.Contains(request.Id))
                {
                    throw new UnauthorizedAccessException("Доступ запрещён: источник не принадлежит пользователю");
                }
            }

            return source.ToDto();
        }
    }
}
