namespace WebApp.Infrastructure;

public class LogWhenTodoItemDeletedHandler(ILogger<LogWhenTodoItemDeletedHandler> logger)
    : IHandleMessages<TodoItemDeletedEvent>
{
    public Task Handle(TodoItemDeletedEvent message, IMessageHandlerContext context)
    {
        logger.LogInformation("Todo item with id {Id} was deleted.", message.Id);
        
        return Task.CompletedTask;
    }
}