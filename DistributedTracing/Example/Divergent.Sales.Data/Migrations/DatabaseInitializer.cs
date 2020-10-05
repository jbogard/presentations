using System.Linq;
using Divergent.Sales.Data.Context;

namespace Divergent.Sales.Data.Migrations
{
    public static class DatabaseInitializer 
    {
        public static void Initialize(SalesContext context)
        {
            context.Database.EnsureCreated();

            if (context.Products.Any())
            {
                return;
            }

            context.Products.AddRange(SeedData.Products().ToArray());

            context.Orders.AddRange(SeedData.Orders().ToArray());
        }
    }
}
