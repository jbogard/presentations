using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdventureWorksDistributed.UI.Pages;

public class IndexModel : PageModel
{
    public Task OnGet()
    {
        return Task.CompletedTask;
    }
}