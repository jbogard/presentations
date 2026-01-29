#region Example 2

using Data;
using FastEndpoints;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using WebApp.Endpoints.Todo;
using Xunit.Abstractions;

namespace IntegrationTests.Endpoints.Todo;

public class GetByIdTests(WebAppFixture App, ITestOutputHelper Output)
    : WebAppTestBase(App, Output)
{
    [Fact]
    public async Task Get_returns_single_item()
    {
        // Arrange
        var todoItem = new TodoItem { Name = "Test Item 1" };
        await ExecuteDbContextAsync(async dbContext =>
        {
            dbContext.TodoItems.Add(todoItem);
            
            await dbContext.SaveChangesAsync();
        });

        this.App.Services.GetRequiredService<GetByIdEndpoint>();
        
        // Act
        TodoItem? response = null;
        await ExecuteDbContextAsync(async db =>
        {
            var endpoint = Factory.Create<GetByIdEndpoint>(
                ctx => ctx.Request.RouteValues.Add("id", todoItem.Id.ToString()),
                db
            );
            await endpoint.HandleAsync(CancellationToken.None);

            response = endpoint.Response;
        });
        
        // Assert
        response.ShouldNotBeNull();
        response.Id.ShouldBe(todoItem.Id);
    }
}
#endregion