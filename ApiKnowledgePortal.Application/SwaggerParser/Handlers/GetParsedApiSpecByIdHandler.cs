using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ApiKnowledgePortal.Application.Abstractions.Persistence;
using ApiKnowledgePortal.Application.SwaggerParser.Dtos;
using ApiKnowledgePortal.Application.SwaggerParser.Queries;
using ApiKnowledgePortal.Domain.Users;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace ApiKnowledgePortal.Application.SwaggerParser.Handlers
{
    public class GetParsedApiSpecByIdHandler : IRequestHandler<GetParsedApiSpecByIdQuery, ParsedApiSpecDto>
    {
        private readonly IParsedApiSpecRepository _repository;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepo;
        private readonly IApiSpecRepository _apiSpecRepo;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetParsedApiSpecByIdHandler(
            IParsedApiSpecRepository repository,
            IMapper mapper
            , IUserRepository userRepo
            , IApiSpecRepository apiSpecRepo
            , IHttpContextAccessor httpContextAccessor
        )
        {
            _repository = repository;
            _mapper = mapper;
            _userRepo = userRepo;
            _apiSpecRepo = apiSpecRepo;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ParsedApiSpecDto> Handle(GetParsedApiSpecByIdQuery request, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (entity is null)
                throw new KeyNotFoundException("спаршенная апи спецификация не найдена");

            // проверка доступа пользователя к апи спецификации если он не админ
            var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim != null && Guid.TryParse(userIdClaim.Value, out var userId))
            {
                var user = await _userRepo.GetByIdAsync(userId, cancellationToken);
                if (user != null && user.Role != UserRole.Admin)
                {
                    var apiSpec = await _apiSpecRepo.GetByIdAsync(new ApiKnowledgePortal.Domain.ValueObjects.ApiSpecId(entity.ApiSpecId), cancellationToken);
                    if (apiSpec == null || apiSpec.SwaggerSourceId == null || !user.Sources.Contains(apiSpec.SwaggerSourceId.Value))
                    {
                        throw new UnauthorizedAccessException("Доступ запрещён: эндпоинт не принадлежит разрешённому источнику");
                    }
                }
            }

            return _mapper.Map<ParsedApiSpecDto>(entity);
        }
    }
}