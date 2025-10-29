#region Example 2

using Data;
using FastEndpoints;
using Shouldly;
using WebApp.Endpoints.Todo;
using Xunit.Abstractions;

namespace IntegrationTests.Endpoints.Todo;

public class GetTests(WebAppFixture App, ITestOutputHelper Output)
    : WebAppTestBase(App, Output)
{
    [Fact]
    public async Task Get_returns_all_items()
    {
        // Arrange
        await ExecuteDbContextAsync(async dbContext =>
        {
            dbContext.TodoItems.Add(new TodoItem { Name = "Test Item 1" });
            dbContext.TodoItems.Add(new TodoItem { Name = "Test Item 2" });
            await dbContext.SaveChangesAsync();
        });
        
        // Act
        var (response, result) = await App.Client.GETAsync<GetEndpoint, TodoItem[]>();
        
        // Assert
        response.IsSuccessStatusCode.ShouldBeTrue();
        result.Length.ShouldBeGreaterThanOrEqualTo(2); 
        // Considering other tests might have added items
    }
}
#endregion
