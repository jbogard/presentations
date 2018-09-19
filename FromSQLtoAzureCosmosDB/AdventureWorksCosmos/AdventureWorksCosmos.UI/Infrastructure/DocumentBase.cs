using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace AdventureWorksCosmos.UI.Infrastructure
{
    public abstract class DocumentBase
    {
        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; set; }

        private HashSet<IDocumentMessage> _outbox 
            = new HashSet<IDocumentMessage>(DocumentMessageEqualityComparer.Instance);
        private HashSet<IDocumentMessage> _inbox
            = new HashSet<IDocumentMessage>(DocumentMessageEqualityComparer.Instance);

        public IEnumerable<IDocumentMessage> Outbox
        {
            get => _outbox;
            protected set => _outbox = value == null
                ? new HashSet<IDocumentMessage>(DocumentMessageEqualityComparer.Instance)
                : new HashSet<IDocumentMessage>(value, DocumentMessageEqualityComparer.Instance);
        }

        public IEnumerable<IDocumentMessage> Inbox
        {
            get => _inbox;
            protected set => _inbox = value == null
                ? new HashSet<IDocumentMessage>(DocumentMessageEqualityComparer.Instance)
                : new HashSet<IDocumentMessage>(value, DocumentMessageEqualityComparer.Instance);
        }

        protected void Send(IDocumentMessage documentMessage)
        {
            if (_outbox == null)
                _outbox = new HashSet<IDocumentMessage>(DocumentMessageEqualityComparer.Instance);

            _outbox.Add(documentMessage);
        }

        protected void Process<TDocumentMessage>(
            TDocumentMessage documentMessage, 
            Action<TDocumentMessage> action)
            where TDocumentMessage : IDocumentMessage
        {
            if (_inbox == null)
                _inbox = new HashSet<IDocumentMessage>(DocumentMessageEqualityComparer.Instance);

            if (_inbox.Contains(documentMessage))
                return;

            action(documentMessage);

            _inbox.Add(documentMessage);
        }

        public void ProcessDocumentMessage(
            IDocumentMessage documentMessage)
        {
            _outbox?.Remove(documentMessage);
        }
    }
}