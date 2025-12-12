using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiKnowledgePortal.Domain.SwaggerSources;
using ApiKnowledgePortal.Domain.ValueObjects;

namespace ApiKnowledgePortal.Domain.ApiSpecifications
{
    public class ApiSpecifications
    {
        public ApiSpecId Id { get; private set; }
        public Guid? SwaggerSourceId { get; private set; }
        public string Name { get; private set; }
        public string Version { get; private set; }
        public string Content { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public ApiSpecifications(ApiSpecId id, Guid swaggerSourceId, string name, string version, string content)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));

            SwaggerSourceId = swaggerSourceId;

            Name = !string.IsNullOrWhiteSpace(name)
                ? name
                : throw new ArgumentException("название не может быть пустым", nameof(name));
            Version = !string.IsNullOrWhiteSpace(version)
                ? version
                : throw new ArgumentException("версия не может быть пустой", nameof(version));
            Content = !string.IsNullOrWhiteSpace(content)
                ? content
                : throw new ArgumentException("контент не может быть пустым", nameof(content));
            CreatedAt = DateTime.UtcNow;
        }

        private ApiSpecifications() { }

        public void UpdateContent(string newContent)
        {
            if (string.IsNullOrWhiteSpace(newContent))
                throw new ArgumentException("контент не может быть пустым", nameof(newContent));

            Content = newContent;
        }
    }
}