using System;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace AdventureWorksCosmos.UI.Infrastructure
{
    public class DocumentMessageDispatcher : IDocumentMessageDispatcher
    {
        private readonly ServiceFactory _serviceFactory;

        public DocumentMessageDispatcher(ServiceFactory serviceFactory)
        {
            _serviceFactory = serviceFactory;
        }

        public async Task<Exception> Dispatch(DocumentBase document)
        {
            var repository = GetRepository(document.GetType());
            foreach (var documentMessage in document.Outbox.ToArray())
            {
                try
                {
                    var handler = GetHandler(documentMessage);

                    await handler.Handle(documentMessage, _serviceFactory);

                    document.ProcessDocumentMessage(documentMessage);

                    await repository.Update(document);
                }
                catch (Exception ex)
                {
                    return ex;
                }
            }
            return null;
        }

        private static DomainEventDispatcherHandler GetHandler(
            IDocumentMessage documentMessage)
        {
            var genericDispatcherType = typeof(DomainEventDispatcherHandler<>)
                .MakeGenericType(documentMessage.GetType());

            return (DomainEventDispatcherHandler)
                Activator.CreateInstance(genericDispatcherType);
        }

        private DocumentDbRepo GetRepository(Type aggregateType)
        {
            var repoBaseType = typeof(DocumentDbRepo<>).MakeGenericType(aggregateType);
            var repoType = typeof(IDocumentDBRepository<>).MakeGenericType(aggregateType);
            var repoInstance = _serviceFactory(repoType);

            return (DocumentDbRepo)Activator.CreateInstance(repoBaseType, repoInstance);
        }

        private abstract class DocumentDbRepo
        {
            public abstract Task<DocumentBase> FindById(Guid id);
            public abstract Task Update(DocumentBase document);
        }

        private class DocumentDbRepo<T> : DocumentDbRepo
            where T : DocumentBase
        {
            private readonly IDocumentDBRepository<T> _repository;

            public DocumentDbRepo(IDocumentDBRepository<T> repository)
            {
                _repository = repository;
            }

            public override async Task<DocumentBase> FindById(Guid id)
            {
                var root = await _repository.GetItemAsync(id);

                return root;
            }

            public override Task Update(DocumentBase document)
            {
                return _repository.UpdateItemAsync((T)document);
            }
        }

        private abstract class DomainEventDispatcherHandler
        {
            public abstract Task Handle(
                IDocumentMessage documentMessage, 
                ServiceFactory factory);
        }

        private class DomainEventDispatcherHandler<T> : DomainEventDispatcherHandler
            where T : IDocumentMessage
        {
            public override Task Handle(IDocumentMessage documentMessage, ServiceFactory factory)
            {
                return HandleCore((T)documentMessage, factory);
            }

            private static async Task HandleCore(T domainEvent, ServiceFactory factory)
            {
                var handlers = factory.GetInstances<IDocumentMessageHandler<T>>();
                foreach (var handler in handlers)
                {
                    await handler.Handle(domainEvent);
                }
            }
        }
    }
}