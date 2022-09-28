using System.Threading.Tasks;
using AdventureWorksDistributed.Products.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AdventureWorksDistributed.UI.ViewComponents;

public class CategoriesViewComponent : ViewComponent
{
    private AdventureWorks2016Context _db;

    public CategoriesViewComponent(AdventureWorks2016Context db) => _db = db;

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var categories = await _db.ProductCategory.ToListAsync();

        return View(categories);
    }
}