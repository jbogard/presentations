using System;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace AdventureWorksCosmos.UI.Infrastructure
{
    public class DomainEventDispatcher : IDomainEventDispatcher
    {
        private readonly ServiceFactory _serviceFactory;

        public DomainEventDispatcher(ServiceFactory serviceFactory)
        {
            _serviceFactory = serviceFactory;
        }

        public async Task<Exception> Dispatch(AggregateBase aggregate)
        {
            var repository = GetRepository(aggregate.GetType());
            foreach (var domainEvent in aggregate.Outbox.ToArray())
            {
                try
                {
                    var handler = GetHandler(domainEvent);

                    await handler.Handle(domainEvent, _serviceFactory);

                    aggregate.ProcessDomainEvent(domainEvent);

                    await repository.Update(aggregate);
                }
                catch (Exception ex)
                {
                    return ex;
                }
            }
            return null;
        }

        private static DomainEventDispatcherHandler GetHandler(IDomainEvent domainEvent)
        {
            return (DomainEventDispatcherHandler)
                Activator.CreateInstance(typeof(DomainEventDispatcherHandler<>).MakeGenericType(domainEvent.GetType()));
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
            public abstract Task<AggregateBase> FindById(Guid id);
            public abstract Task Update(AggregateBase aggregate);
        }

        private class DocumentDbRepo<T> : DocumentDbRepo
            where T : AggregateBase
        {
            private readonly IDocumentDBRepository<T> _repository;

            public DocumentDbRepo(IDocumentDBRepository<T> repository)
            {
                _repository = repository;
            }

            public override async Task<AggregateBase> FindById(Guid id)
            {
                var root = await _repository.GetItemAsync(id);

                return root;
            }

            public override Task Update(AggregateBase aggregate)
            {
                return _repository.UpdateItemAsync((T)aggregate);
            }
        }

        private abstract class DomainEventDispatcherHandler
        {
            public abstract Task Handle(IDomainEvent domainEvent, ServiceFactory factory);
        }

        private class DomainEventDispatcherHandler<T> : DomainEventDispatcherHandler
            where T : IDomainEvent
        {
            public override Task Handle(IDomainEvent domainEvent, ServiceFactory factory)
            {
                return HandleCore((T)domainEvent, factory);
            }

            private static async Task HandleCore(T domainEvent, ServiceFactory factory)
            {
                var handlers = factory.GetInstances<IDomainEventHandler<T>>();
                foreach (var handler in handlers)
                {
                    await handler.Handle(domainEvent);
                }
            }
        }
    }
}