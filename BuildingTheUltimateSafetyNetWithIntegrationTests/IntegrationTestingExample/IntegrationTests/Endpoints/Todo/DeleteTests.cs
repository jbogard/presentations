#region Example 7

using Data;
using FastEndpoints;
using Messages;
using NServiceBus.Testing;
using Shouldly;
using WebApp.Endpoints.Todo;

namespace IntegrationTests.Endpoints.Todo;

public class DeleteTests(WebAppFixture App)
    : WebAppTestBase(App)
{
    [Fact]
    public async Task Delete_deletes_item_and_publishes_event()
    {
        // Arrange
        var todoItem = new TodoItem { Name = "Test Item 1" };
        await ExecuteDbContextAsync(async dbContext =>
        {
            dbContext.TodoItems.Add(todoItem);
            
            await dbContext.SaveChangesAsync();
        });
        
        // Act
        var messageSession = new TestableMessageSession();
        await ExecuteDbContextAsync(async db =>
        {
            var endpoint = Factory.Create<DeleteEndpoint>(
                ctx => ctx.Request.RouteValues.Add("id", todoItem.Id.ToString()),
                db,
                messageSession
            );
            await endpoint.HandleAsync(CancellationToken.None);
        });
        
        // Assert
        await ExecuteDbContextAsync(async dbContext =>
        {
            var deletedItem = await dbContext.TodoItems.FindAsync(todoItem.Id);
            deletedItem.ShouldBeNull();
        });
        var publishedEvents = messageSession.PublishedMessages;
        publishedEvents.ShouldNotBeEmpty();
        var todoItemDeletedEvent = publishedEvents
            .Where(publishedEvent => publishedEvent.Message is TodoItemDeletedEvent)
            .Select(publishedEvent => publishedEvent.Message as TodoItemDeletedEvent)
            .ShouldHaveSingleItem()!;
        
        todoItemDeletedEvent.Id.ShouldBe(todoItem.Id);
    }
}
#endregion