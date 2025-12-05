using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiKnowledgePortal.Application.SwaggerSources.Dtos;
using MediatR;

namespace ApiKnowledgePortal.Application.SwaggerSources.Commands
{
    public record CreateSwaggerSourceCommand(string Name, string Url) : IRequest<SwaggerSourceDto>;
}