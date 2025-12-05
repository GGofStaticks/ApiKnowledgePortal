using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiKnowledgePortal.Application.SwaggerSources.Dtos;
using ApiKnowledgePortal.Domain.SwaggerSources;

namespace ApiKnowledgePortal.Application.SwaggerSources.Mapping
{
    public static class SwaggerSourceMapping
    {
        public static SwaggerSourceDto ToDto(this SwaggerSource source)
        {
            return new SwaggerSourceDto
            {
                Id = source.Id,
                Name = source.Name,
                Url = source.Url,
                IsActive = source.IsActive,
                LastFetchTime = source.LastFetchTime,
                LastFetchStatus = source.LastFetchStatus
            };
        }
    }
}