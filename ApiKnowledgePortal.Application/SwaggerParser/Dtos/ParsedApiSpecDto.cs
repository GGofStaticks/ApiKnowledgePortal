using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiKnowledgePortal.Application.SwaggerParser.Dtos
{
    public class ParsedApiSpecDto
    {
        public Guid Id { get; set; }
        public Guid ApiSpecId { get; set; }
        public string Path { get; set; } = default!;
        public string Method { get; set; } = default!;
        public string OperationId { get; set; } = default!;
        public string Summary { get; set; } = default!;
        public DateTime CreatedAt { get; set; }
    }
}