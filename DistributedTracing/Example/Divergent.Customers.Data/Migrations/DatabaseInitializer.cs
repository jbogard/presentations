using System.Linq;
using Divergent.Customers.Data.Context;

namespace Divergent.Customers.Data.Migrations
{
    public static class DatabaseInitializer
    {
        public static void Initialize(CustomersContext context)
        {
            context.Database.EnsureCreated();

            if (context.Customers.Any())
            {
                return;
            }

            context.Customers.AddRange(SeedData.Customers().ToArray());
        }
    }
}
