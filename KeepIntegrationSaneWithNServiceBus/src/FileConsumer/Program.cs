using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;

namespace FileConsumer
{
    class Program
    {
        private static readonly string _connectionString = @"Data Source=.\SQLEXPRESS2008;Initial Catalog=KeepingIntegrationsSane;Integrated Security=SSPI;";

        static void Main()
        {
            ClearData();

            var contents = ReadFileContents();

            ProcessContents(contents);

            MarkFileAsCompleted();
        }

        private static IEnumerable<ProductPurchased> ReadFileContents()
        {
            Console.WriteLine("Reading file contents...");

            var path = Path.Combine(Environment.CurrentDirectory, "ProductData.csv");

            var fileContents = File.ReadAllLines(path);

            return fileContents
                .Select(line => line.Split(','))
                .Select(split => new ProductPurchased
                {
                    TransactionId = split[0],
                    Quantity = Convert.ToInt32(split[1]),
                    Price = Convert.ToDecimal(split[2]),
                    Sku = split[3]
                })
                .ToArray();
        }

        private static void ProcessContents(IEnumerable<ProductPurchased> productsPurchased)
        {
            Console.WriteLine("Processing file contents...");

            foreach (var item in productsPurchased)
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    conn.Open();

                    using (var tx = conn.BeginTransaction())
                    using (var cmd = new SqlCommand("INSERT ProductPurchased (TransactionId, Quantity, Price, Sku) VALUES (@TransactionId, @Quantity, @Price, @Sku)", conn, tx))
                    {
                        cmd.Parameters.AddWithValue("TransactionId", item.TransactionId);
                        cmd.Parameters.AddWithValue("Quantity", item.Quantity);
                        cmd.Parameters.AddWithValue("Price", item.Price);
                        cmd.Parameters.AddWithValue("Sku", item.Sku);
                        cmd.ExecuteNonQuery();

                        tx.Commit();
                    }
                }
            }
        }

        private static void MarkFileAsCompleted()
        {
            Console.WriteLine("Marking file as completed...");

            var oldPath = Path.Combine(Environment.CurrentDirectory, "ProductData.csv");
            var newPath = Path.Combine(Environment.CurrentDirectory, "ProductData.csv_processed");

            if (File.Exists(newPath))
                File.Delete(newPath);

            File.Move(oldPath, newPath);
        }

        private static void ClearData()
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                using (var tx = conn.BeginTransaction())
                using (var cmd = new SqlCommand("DELETE ProductPurchased", conn, tx))
                {
                    cmd.ExecuteNonQuery();

                    tx.Commit();
                }
            }
        }
    }
}
