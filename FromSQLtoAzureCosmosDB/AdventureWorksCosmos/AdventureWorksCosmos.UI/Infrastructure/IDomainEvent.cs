using System;

namespace AdventureWorksCosmos.UI.Infrastructure
{
    public interface IDomainEvent
    {
        Guid Id { get; }
    }
}