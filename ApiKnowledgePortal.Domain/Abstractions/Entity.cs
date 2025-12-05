using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace ApiKnowledgePortal.Domain.Abstractions
{
    public interface IHasDomainEvents
    {
        IReadOnlyCollection<INotification> DomainEvents { get; }
        void ClearDomainEvents();
    }

    public abstract class Entity<TId> : IHasDomainEvents
    {
        public TId Id { get; protected set; } = default!;

        private readonly List<INotification> _domainEvents = new();
        public IReadOnlyCollection<INotification> DomainEvents => _domainEvents.AsReadOnly();

        protected void Raise(INotification domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }

        public void ClearDomainEvents() => _domainEvents.Clear();
    }

    public abstract class AggregateRoot<TId> : Entity<TId>
    {
    }
}