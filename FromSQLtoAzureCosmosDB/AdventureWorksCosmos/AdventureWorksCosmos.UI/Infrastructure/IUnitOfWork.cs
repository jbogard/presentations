using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdventureWorksCosmos.UI.Infrastructure
{
    public interface IUnitOfWork
    {
        T Find<T>(Guid id) where T : AggregateBase;
        void Register(AggregateBase aggregate);
        void Register(IEnumerable<AggregateBase> aggregates);
        Task Complete();
    }
}