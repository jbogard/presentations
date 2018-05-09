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
    public class IndexModel : PageModel
    {
        private readonly AdventureWorks2016Context _db;
        public IndexModel(AdventureWorks2016Context db) => _db = db;

        public async Task OnGet(int id)
        {
            Category = await _db.ProductCategory
                .Include(pc => pc.ProductSubcategory)
                .SingleAsync(c => c.ProductCategoryId == id);
        }

        public ProductCategory Category { get; set; }
    }
}