using System;
using Neo4jClient;
using Polyglot.Products.Models;
using Product = Polyglot.OrderAnalytics.Model.Product;

namespace Polyglot.OrderAnalytics.Loader
{
    class Program
    {
        static void Main(string[] args)
        {
            var graphClient = new GraphClient(new Uri("http://localhost:7474/db/data"));
            graphClient.Connect();


            Fill(graphClient);
        }

        private static void Fill(GraphClient graphClient)
        {
            using (var context = new AdventureWorks2012Context())
            {
                foreach (var product in context.Products)
                {
                    graphClient.Create(new Product {ProductId = product.ProductID});
                }
            }
        }
    }
}
