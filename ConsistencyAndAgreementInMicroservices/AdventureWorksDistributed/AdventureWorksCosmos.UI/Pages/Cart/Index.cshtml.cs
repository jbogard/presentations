using AdventureWorksDistributed.Cart.Contracts;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdventureWorksDistributed.UI.Pages.Cart;

public class IndexModel : PageModel
{
    public ShoppingCart Cart { get; private set; }

    public void OnGet()
    {
        Cart = HttpContext.Session.Get<ShoppingCart>("Cart") ?? new ShoppingCart();
    }
}