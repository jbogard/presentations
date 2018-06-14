using System;
using System.Threading.Tasks;

namespace AdventureWorksCosmos.UI.Infrastructure
{
    public interface IDomainEventDispatcher
    {
        Task<Exception> Dispatch(AggregateBase aggregate);
    }
}