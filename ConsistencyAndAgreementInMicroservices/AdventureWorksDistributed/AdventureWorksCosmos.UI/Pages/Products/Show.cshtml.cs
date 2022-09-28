using System.Threading.Tasks;
using AdventureWorksDistributed.Products.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AdventureWorksDistributed.UI.Pages.Products;

public class ShowModel : PageModel
{
    private readonly AdventureWorks2016Context _db;

    public ShowModel(AdventureWorks2016Context db) => _db = db;

    public Product Product { get; private set; }

    public async Task OnGet(int id)
    {
        Product = await _db.Product
            .Include("ProductModel.ProductModelProductDescriptionCulture.ProductDescription")
            .SingleAsync(c => c.ProductId == id);
    }
}