using NServiceBus;

namespace Messages;

public class TodoItemDeletedEvent : IEvent
{
    public required long Id { get; init;  }
}