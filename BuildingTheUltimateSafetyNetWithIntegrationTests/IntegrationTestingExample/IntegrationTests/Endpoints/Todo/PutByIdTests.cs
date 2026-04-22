#region Example 3

using Data;
using FastEndpoints;
using Shouldly;
using WebApp.Endpoints.Todo;

namespace IntegrationTests.Endpoints.Todo;

public class PutByIdTests(WebAppFixture App)
    : WebAppTestBase(App)
{
    [Fact]
    public async Task Put_updates_single_item()
    {
        // Arrange
        var todoItem = new TodoItem { Name = "Test Item 1" };
        await ExecuteDbContextAsync(async dbContext =>
        {
            dbContext.TodoItems.Add(todoItem);
            
            await dbContext.SaveChangesAsync();
        });
        
        // Act
        var request = new PutEndpoint.Request
        {
            Name = "Updated Item",
            IsComplete = true
        };
        await ExecuteDbContextAsync(async db =>
        {
            var endpoint = Factory.Create<PutEndpoint>(
                ctx => ctx.Request.RouteValues.Add("id", todoItem.Id.ToString()),
                db
            );
            await endpoint.HandleAsync(request, CancellationToken.None);
        });
        
        // Assert
        await ExecuteDbContextAsync(async dbContext =>
        {
            var updatedItem = await dbContext.TodoItems.FindAsync(todoItem.Id);
            updatedItem.ShouldNotBeNull();
            updatedItem.Name.ShouldBe("Updated Item");
            updatedItem.IsComplete.ShouldBeTrue();
        });
    }
}
#endregion