using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdventureWorksCosmos.Products.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AdventureWorksCosmos.UI.Pages.Products
{
    public class ListModel : PageModel
    {
        private readonly AdventureWorks2016Context _db;

        public ListModel(AdventureWorks2016Context db) => _db = db;

        public async Task OnGet(int id)
        {
            Subcategory = await _db.ProductSubcategory
                .Include(p => p.Product)
                .SingleAsync(c => c.ProductSubcategoryId == id);

        }

        public ProductSubcategory Subcategory { get; set; }
    }
}