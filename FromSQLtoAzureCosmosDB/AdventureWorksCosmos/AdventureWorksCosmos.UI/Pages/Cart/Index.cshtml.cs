using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdventureWorksCosmos.UI.Pages.Cart
{
    public class IndexModel : PageModel
    {
        public ShoppingCart Cart { get; private set; }

        public void OnGet()
        {
            Cart = HttpContext.Session.Get<ShoppingCart>("Cart") ?? new ShoppingCart();
        }
    }
}