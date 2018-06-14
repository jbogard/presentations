using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdventureWorksCosmos.UI.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDomainEventDispatcher _dispatcher;
        private readonly ISet<AggregateBase> _identityMap = new HashSet<AggregateBase>(AggregateBaseEqualityComparer.Instance);

        public UnitOfWork(IDomainEventDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        public void Register(AggregateBase aggregate)
        {
            if (aggregate != null)
            {
                _identityMap.Add(aggregate);
            }
        }

        public void Register(IEnumerable<AggregateBase> aggregates)
        {
            foreach (var aggregate in aggregates)
            {
                Register(aggregate);
            }
        }

        public T Find<T>(Guid id) where T : AggregateBase
        {
            return _identityMap.OfType<T>().FirstOrDefault(ab => ab.Id == id);
        }

        public async Task Complete()
        {
            var toSkip = new HashSet<AggregateBase>(AggregateBaseEqualityComparer.Instance);

            while (_identityMap.Except(toSkip, AggregateBaseEqualityComparer.Instance).Any(a => a.Outbox.Any()))
            {
                var aggregate = _identityMap.Except(toSkip, AggregateBaseEqualityComparer.Instance).FirstOrDefault(a => a.Outbox.Any());

                if (aggregate == null)
                    continue;

                var ex = await _dispatcher.Dispatch(aggregate);
                if (ex != null)
                {
                    toSkip.Add(aggregate);
                    // TODO: Retry offline
                }
            }
        }
    }
}