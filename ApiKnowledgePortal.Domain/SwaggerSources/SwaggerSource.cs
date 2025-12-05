using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiKnowledgePortal.Domain.SwaggerSources
{
    public class SwaggerSource
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; } = default!;
        public string Url { get; private set; } = default!;
        public bool IsActive { get; private set; }
        public DateTime? LastFetchTime { get; private set; }
        public string? LastFetchStatus { get; private set; }

        // Конструктор для EF / создание
        private SwaggerSource() { }

        public SwaggerSource(Guid id, string name, string url)
        {
            Id = id == Guid.Empty ? Guid.NewGuid() : id;
            Name = !string.IsNullOrWhiteSpace(name) ? name : throw new ArgumentException("название не может быть пустым", nameof(name));
            Url = !string.IsNullOrWhiteSpace(url) ? url : throw new ArgumentException("урл не может быть пустым", nameof(url));
            IsActive = true;
        }

        public void MarkFetched(string status)
        {
            LastFetchTime = DateTime.UtcNow;
            LastFetchStatus = status;
        }

        public void Deactivate() => IsActive = false;
        public void Activate() => IsActive = true;

        public void UpdateUrl(string url)
        {
            Url = !string.IsNullOrWhiteSpace(url) ? url : throw new ArgumentException("урл не может быть пустым", nameof(url));
        }

        public void UpdateName(string name)
        {
            Name = !string.IsNullOrWhiteSpace(name) ? name : throw new ArgumentException("название не может быть пустым", nameof(name));
        }
    }
}