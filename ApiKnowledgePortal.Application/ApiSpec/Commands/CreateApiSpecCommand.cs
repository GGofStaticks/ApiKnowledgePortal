using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiKnowledgePortal.Application.ApiSpec.Dtos;
using MediatR;

namespace ApiKnowledgePortal.Application.ApiSpec.Commands
{
    public record CreateApiSpecCommand(string Name, string Version, string Content) : IRequest<ApiSpecDto>;
}