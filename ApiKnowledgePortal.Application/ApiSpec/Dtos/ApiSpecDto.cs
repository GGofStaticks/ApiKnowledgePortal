using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiKnowledgePortal.Application.ApiSpec.Dtos
{
    public class ApiSpecDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public string Version { get; set; } = default!;
        public string Content { get; set; } = default!;
        public DateTime CreatedAt { get; set; }
    }
}