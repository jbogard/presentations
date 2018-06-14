using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdventureWorksCosmos.UI.Models.Orders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdventureWorksCosmos.UI.Pages.Orders
{
    public class ShowModel : PageModel
    {
        private readonly IDocumentDBRepository<OrderRequest> _db;

        public ShowModel(IDocumentDBRepository<OrderRequest> db) => _db = db;

        public async Task OnGet(Guid id)
        {
            Order = await _db.GetItemAsync(id);
        }

        public OrderRequest Order { get; set; }
    }
}