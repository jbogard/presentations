using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdventureWorksCosmos.UI.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDocumentMessageDispatcher _dispatcher;
        private readonly ISet<DocumentBase> _identityMap 
            = new HashSet<DocumentBase>(DocumentBaseEqualityComparer.Instance);

        public UnitOfWork(IDocumentMessageDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        public void Register(DocumentBase document)
        {
            if (document != null)
            {
                _identityMap.Add(document);
            }
        }

        public void Register(IEnumerable<DocumentBase> aggregates)
        {
            foreach (var aggregate in aggregates)
            {
                Register(aggregate);
            }
        }

        public T Find<T>(Guid id) where T : DocumentBase
        {
            return _identityMap.OfType<T>().FirstOrDefault(ab => ab.Id == id);
        }

        public async Task Complete()
        {
            var toSkip = new HashSet<DocumentBase>(DocumentBaseEqualityComparer.Instance);

            while (_identityMap.Except(toSkip, DocumentBaseEqualityComparer.Instance).Any(a => a.Outbox.Any()))
            {
                var aggregate = _identityMap.Except(toSkip, DocumentBaseEqualityComparer.Instance).FirstOrDefault(a => a.Outbox.Any());

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