using Microsoft.EntityFrameworkCore;
using WebApp.Data;

namespace WebApp.Infrastructure;

public class TodoWreckerJob(ILogger<TodoWreckerJob> logger,
    WebAppDbContext dbContext)
{
    public async Task DoWork(CancellationToken ct)
    {
        logger.LogInformation("Marking all todo items incomplete...");

        var items = await dbContext.TodoItems
            .Where(item => item.IsComplete)
            .ToListAsync(ct);

        foreach (var todoItem in items)
        {
            todoItem.IsComplete = false;
        }

        await dbContext.SaveChangesAsync(ct);

        logger.LogInformation("Marked {count} todo items incomplete.", items.Count);
    }
}