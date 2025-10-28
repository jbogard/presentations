namespace WebApp.Infrastructure;

public class TodoItemDeletedEvent : IEvent
{
    public required long Id { get; init;  }
}