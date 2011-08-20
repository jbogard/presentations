using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;

namespace FileProducer
{
    class Program
    {
        private static readonly string _connectionString = @"Data Source=.\SQLEXPRESS2008;Initial Catalog=KeepingIntegrationsSane;Integrated Security=SSPI;";

        static void Main()
        {
            ResetData();

            var contents = GetOrdersProcessed();

            WriteContentsToFile(contents);

            MarkAsExported();
        }

        private static IEnumerable<string> GetOrdersProcessed()
        {
            Console.WriteLine("Retrieving orders...");

            var items = new List<string>();

            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                using (var tx = conn.BeginTransaction())
                using (var cmd = new SqlCommand("SELECT OrderId, PurchaseDate, Amount FROM OrdersProcessed WHERE IsExported = 0", conn, tx))
                using (var reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (reader.Read())
                    {
                        var values = new object[3];
                        reader.GetValues(values);
                        items.Add(string.Join(",", values.Select(v => v.ToString())));
                    }
                }
            }

            return items;
        }

        private static void WriteContentsToFile(IEnumerable<string> contents)
        {
            Console.WriteLine("Writing orders to disk...");

            string path = Path.Combine(Environment.CurrentDirectory, "orders_processed.csv");

            if (File.Exists(path))
                File.Delete(path);

            File.WriteAllLines(path, contents);
        }

        private static void MarkAsExported()
        {
            Console.WriteLine("Marking orders as exported...");

            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                using (var tx = conn.BeginTransaction())
                using (var cmd = new SqlCommand("UPDATE OrdersProcessed SET IsExported = 1", conn, tx))
                {
                    cmd.ExecuteNonQuery();

                    tx.Commit();
                }
            }
        }

        private static void ResetData()
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                using (var tx = conn.BeginTransaction())
                using (var cmd = new SqlCommand("UPDATE OrdersProcessed SET IsExported = 0", conn, tx))
                {
                    cmd.ExecuteNonQuery();

                    tx.Commit();
                }
            }
        }
    }
}
