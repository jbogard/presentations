#region Example 6

using Data;
using IntegrationTests.Endpoints;
using Microsoft.EntityFrameworkCore;
using Shouldly;

namespace IntegrationTests.Jobs;

public class TodoWreckerJobTests(WebAppFixture App)
    : WebAppTestBase(App)
{
    [Fact]
    public async Task Wrecker_wrecks_all_items()
    {
        // Arrange
        await ExecuteDbContextAsync(async dbContext =>
        {
            dbContext.TodoItems.Add(new TodoItem { Name = "Test Item 1" });
            dbContext.TodoItems.Add(new TodoItem { Name = "Test Item 2" });
            await dbContext.SaveChangesAsync();
        });
        
        // Act
        var result = await ExecuteDbContextAsync(async db =>
            await db.TodoItems.Where(item => item.IsComplete == true).ToListAsync()
        );
        
        // Assert
        result.Count.ShouldBe(0);
    }
}
#endregion