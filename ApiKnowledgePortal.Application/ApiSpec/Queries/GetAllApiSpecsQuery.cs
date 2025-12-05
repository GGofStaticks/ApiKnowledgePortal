using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiKnowledgePortal.Application.ApiSpec.Dtos;
using MediatR;

namespace ApiKnowledgePortal.Application.ApiSpec.Queries
{
    public record GetAllApiSpecsQuery() : IRequest<IEnumerable<ApiSpecDto>>;
}
