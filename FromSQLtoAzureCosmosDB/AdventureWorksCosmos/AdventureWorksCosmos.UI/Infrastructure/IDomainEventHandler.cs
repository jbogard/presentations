using System.Threading.Tasks;

namespace AdventureWorksCosmos.UI.Infrastructure
{
    public interface IDomainEventHandler<in T>
        where T : IDomainEvent
    {
        Task Handle(T domainEvent);
    }
}