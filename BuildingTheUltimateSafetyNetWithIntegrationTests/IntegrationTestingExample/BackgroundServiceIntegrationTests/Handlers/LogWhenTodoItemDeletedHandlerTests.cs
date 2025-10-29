using Communication.HandlerIntegrationTests;
using Messages;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;

namespace BackgroundServiceIntegrationTests.Handlers;

public class LogWhenTodoItemDeletedHandlerTests(BackgroundServiceApp App, ITestOutputHelper Output)
    : AppTestBase(App, Output)
{
    [Fact]
    public async Task Should_handle_event()
    {
        // Arrange
        var message = new TodoItemDeletedEvent
        {
            Id = 42
        };
        
        // Act
        var session = App.Services.GetRequiredService<IMessageSession>();
        await ExecuteAndWaitForHandled<TodoItemDeletedEvent>(
            () => session.Publish(message)
        );
        
        // Assert
        // (no exceptions means success)
    }
}