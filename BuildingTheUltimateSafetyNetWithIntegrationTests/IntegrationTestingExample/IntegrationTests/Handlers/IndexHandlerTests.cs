#region Example 5

using Data;
using IntegrationTests.Endpoints;
using Shouldly;

namespace IntegrationTests.Handlers;

public class IndexHandlerTests(WebAppFixture App)
    : WebAppTestBase(App)
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
        var result = await SendAsync(new WebApp.Pages.Index.Request());
        
        // Assert
        result.Count().ShouldBeGreaterThanOrEqualTo(2); 
        // Considering other tests might have added items
    }
}
#endregion