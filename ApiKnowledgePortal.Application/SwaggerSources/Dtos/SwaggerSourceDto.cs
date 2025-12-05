using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiKnowledgePortal.Application.SwaggerSources.Dtos
{
    public class SwaggerSourceDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public string Url { get; set; } = default!;
        public bool IsActive { get; set; }
        public DateTime? LastFetchTime { get; set; }
        public string? LastFetchStatus { get; set; }
    }
}