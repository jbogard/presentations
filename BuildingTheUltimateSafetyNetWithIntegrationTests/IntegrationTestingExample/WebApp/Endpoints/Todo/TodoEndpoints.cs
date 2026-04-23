using Data;
using FastEndpoints;
using Messages;
using Microsoft.EntityFrameworkCore;
using WebApp.Infrastructure;

namespace WebApp.Endpoints.Todo;

#region Example 2

public class GetEndpoint(WebAppDbContext db) : EndpointWithoutRequest<TodoItem[]>
{
    public override void Configure()
    {
        Get("/api/todo");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var items = await db.TodoItems.ToArrayAsync(ct);
        
        await Send.OkAsync(items, ct);
    }
}

public class GetByIdEndpoint(WebAppDbContext db) : EndpointWithoutRequest<TodoItem>
{
    public override void Configure()
    {
        Get("/api/todo/{Id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var todoItemId = Route<long>("Id");
        
        var item =  await db.TodoItems
            .SingleOrDefaultAsync(t => t.Id == todoItemId, ct);
        
        if (item == null)
        {
            await Send.NotFoundAsync(ct);
            
            return;
        }
        
        await Send.OkAsync(item, ct);
    }
}
#endregion

#region Example 3
public class PutEndpoint(WebAppDbContext db) : Endpoint<PutEndpoint.Request>
{
    public class Request
    {
        public string Name { get; set; }
        public bool IsComplete { get; set; }
    }
    
    public override void Configure()
    {
        Put("/api/todo/{Id}");
    }

    public override async Task HandleAsync(Request request, CancellationToken ct)
    {
        var todoItemId = Route<long>("Id");
        
        var item = await db.TodoItems.SingleOrDefaultAsync(t => t.Id == todoItemId, ct);
        
        if (item == null)
        {
            await Send.NotFoundAsync(ct);
            
            return;
        }

        item.Name = request.Name;
        item.IsComplete = request.IsComplete;

        await db.SaveChangesAsync(ct);
        
        await Send.OkAsync(item, ct);
    }
}
#endregion

#region Example 4
public class PostEndpoint(WebAppDbContext db, IWeatherForecastClient client) : Endpoint<PostEndpoint.Request, TodoItem>
{
    public class Request
    {
        public string Name { get; set; }
        public bool IsComplete { get; set; }
    }
    
    public override void Configure()
    {
        Post("/api/todo");
        AllowAnonymous();
    }

    public override async Task HandleAsync(Request request, CancellationToken ct)
    {
        var weather = await client.GetAsync();

        var first = weather?.First();
        
        var item = new TodoItem
        {
            Name = $"{request.Name} - Weather: {first?.Summary ?? "No Data"}",
            IsComplete = request.IsComplete
        };

        db.TodoItems.Add(item);
        
        await db.SaveChangesAsync(ct);
        
        await Send.OkAsync(item, ct);
    }
}
#endregion

#region Example 7
public class DeleteEndpoint(WebAppDbContext db) : EndpointWithoutRequest
{
    public override void Configure()
    {
        Delete("/api/todo/{Id}");

        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var todoItemId = Route<long>("Id");
        
        var count = await db.TodoItems
            .Where(todoItem => todoItem.Id == todoItemId)
            .ExecuteDeleteAsync(ct);
        
        await db.SaveChangesAsync(ct);
        
        if (count > 0)
        {
            var messageSession = HttpContext.RequestServices.GetRequiredService<IMessageSession>();
            await messageSession.Publish(new TodoItemDeletedEvent { Id = todoItemId }, ct);
        }
        
        await Send.NoContentAsync(ct);
    }
}
#endregion