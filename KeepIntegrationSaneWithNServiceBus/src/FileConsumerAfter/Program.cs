using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using FileConsumerAfterMessages;
using NServiceBus;

namespace FileConsumer
{
    class Program
    {
        private static readonly string _connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=KeepingIntegrationsSane;Integrated Security=SSPI;";

        static void Main()
        {
            Bus = Configure.With()
                .Log4Net()
                .DefaultBuilder()
                .XmlSerializer()
                .MsmqTransport()
                .UnicastBus()
                .SendOnly();

            ClearData();

            var contents = ReadFileContents();

            ProcessContents(contents);

            MarkFileAsCompleted();
        }

        protected static IBus Bus { get; set; }

        private static IEnumerable<RecordProductPurchased> ReadFileContents()
        {
            Console.WriteLine("Reading file contents...");

            var path = Path.Combine(Environment.CurrentDirectory, "ProductData.csv");

            var fileContents = File.ReadAllLines(path);

            return fileContents
                .Select(line => line.Split(','))
                .Select(split => new RecordProductPurchased
                {
                    TransactionId = split[0],
                    Quantity = Convert.ToInt32(split[1]),
                    Price = Convert.ToDecimal(split[2]),
                    Sku = split[3]
                })
                .ToArray();
        }

        private static void ProcessContents(IEnumerable<RecordProductPurchased> productsPurchased)
        {
            Console.WriteLine("Processing file contents...");

            foreach (var item in productsPurchased)
            {
                Bus.Send(item);
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
