using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace AdventureWorksCosmos.UI.Infrastructure
{
    public abstract class AggregateBase
    {
        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; set; }

        private HashSet<IDomainEvent> _outbox 
            = new HashSet<IDomainEvent>(DomainEventEqualityComparer.Instance);
        private HashSet<IDomainEvent> _inbox
            = new HashSet<IDomainEvent>(DomainEventEqualityComparer.Instance);

        public IEnumerable<IDomainEvent> Outbox
        {
            get => _outbox;
            protected set => _outbox = value == null
                ? new HashSet<IDomainEvent>(DomainEventEqualityComparer.Instance)
                : new HashSet<IDomainEvent>(value, DomainEventEqualityComparer.Instance);
        }

        public IEnumerable<IDomainEvent> Inbox
        {
            get => _inbox;
            protected set => _inbox = value == null
                ? new HashSet<IDomainEvent>(DomainEventEqualityComparer.Instance)
                : new HashSet<IDomainEvent>(value, DomainEventEqualityComparer.Instance);
        }

        protected void Publish(IDomainEvent domainEvent)
        {
            if (_outbox == null) _outbox = new HashSet<IDomainEvent>(DomainEventEqualityComparer.Instance);
            _outbox.Add(domainEvent);
        }

        protected void Process<TDomainEvent>(TDomainEvent domainEvent, Action<TDomainEvent> action)
            where TDomainEvent : IDomainEvent
        {
            if (_inbox == null) _inbox = new HashSet<IDomainEvent>(DomainEventEqualityComparer.Instance);

            if (_inbox.Contains(domainEvent))
                return;

            action(domainEvent);

            _inbox.Add(domainEvent);
        }

        public void ProcessDomainEvent(IDomainEvent domainEvent)
        {
            _outbox?.Remove(domainEvent);
        }
    }
}