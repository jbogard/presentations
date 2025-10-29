using Data;
using MediatR;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Pages;

public class Index(IMediator mediator) : PageModel
{
    public class Request : IRequest<IEnumerable<TodoItem>> { }
    
    public class Handler(WebAppDbContext dbContext) : IRequestHandler<Request, IEnumerable<TodoItem>>
    {
        public async Task<IEnumerable<TodoItem>> Handle(Request request, CancellationToken cancellationToken)
        {
            return await dbContext.TodoItems.OrderBy(item => item.Id).ToListAsync(cancellationToken);
        }
    }
    
    public IEnumerable<TodoItem> Items { get; set; } = [];
    
    public async Task OnGetAsync()
    {
        Items = await mediator.Send(new Request());
    }
}