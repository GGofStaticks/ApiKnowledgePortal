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
using Microsoft.EntityFrameworkCore;

namespace ApiKnowledgePortal.Application.SwaggerParser.Handlers
{
    public class GetAllParsedApiSpecsHandler : IRequestHandler<GetAllParsedApiSpecsQuery, IEnumerable<ParsedApiSpecDto>>
    {
        private readonly IParsedApiSpecRepository _repository;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepo;
        private readonly IApiSpecRepository _apiSpecRepo;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetAllParsedApiSpecsHandler(
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

        public async Task<IEnumerable<ParsedApiSpecDto>> Handle(GetAllParsedApiSpecsQuery request, CancellationToken cancellationToken)
        {
            var query = _repository.Query();

            var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim != null && Guid.TryParse(userIdClaim.Value, out var userId))
            {
                var user = await _userRepo.GetByIdAsync(userId, cancellationToken);
                if (user != null && user.Role != UserRole.Admin)
                {
                    if (!user.Sources.Any())
                    {
                        query = query.Where(p => false);
                    }
                    else
                    {
                        var allowedApiSpecIds = await _apiSpecRepo.Query()
                            .Where(a => a.SwaggerSourceId != null && user.Sources.Contains(a.SwaggerSourceId.Value))
                            .Select(a => a.Id.Value)
                            .ToListAsync(cancellationToken);

                        query = query.Where(p => allowedApiSpecIds.Contains(p.ApiSpecId));
                    }
                }
            }

            var list = await query.ToListAsync(cancellationToken);
            return _mapper.Map<IEnumerable<ParsedApiSpecDto>>(list);
        }
    }
}
