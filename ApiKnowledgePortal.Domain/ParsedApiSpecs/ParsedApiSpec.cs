using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiKnowledgePortal.Domain.ParsedApiSpecs
{
    public class ParsedApiSpec
    {
        public Guid Id { get; private set; }
        public Guid ApiSpecId { get; private set; } // FK на ApiSpecs
        public string Path { get; private set; } = default!;
        public string Method { get; private set; } = default!;
        public string OperationId { get; private set; } = default!;
        public string Summary { get; private set; } = default!;
        public DateTime CreatedAt { get; private set; }

        private ParsedApiSpec() { }

        public ParsedApiSpec(Guid apiSpecId, string path, string method, string operationId, string summary)
        {
            Id = Guid.NewGuid();
            ApiSpecId = apiSpecId;
            Path = path;
            Method = method;
            OperationId = operationId;
            Summary = summary;
            CreatedAt = DateTime.UtcNow;
        }

        public void UpdateSummary(string summary)
        {
            Summary = summary;
        }
    }
}