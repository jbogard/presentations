using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using FileProducerAfterMessages;
using NServiceBus;
using NServiceBus.Saga;

namespace FileProducerAfter
{
    public class FileProductionSaga : Saga<FileProductionSagaData>,
        IAmStartedByMessages<ExportFile>,
        IHandleMessages<ExportFileContents>
    {
        private static readonly string _connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=KeepingIntegrationsSane;Integrated Security=SSPI;";

        public override void ConfigureHowToFindSaga()
        {
            ConfigureMapping<ExportFileContents>(saga => saga.BatchId, msg => msg.BatchId);
        }

        public void Handle(ExportFile message)
        {
            Data.BatchId = message.BatchId;

            MarkOrdersToExport();

            Bus.SendLocal(new ExportFileContents {BatchId = Data.BatchId});
        }

        public void Handle(ExportFileContents message)
        {
            var contents = GetOrdersProcessed();

            WriteContentsToFile(contents);

            MarkAsComplete();
        }

        private void MarkOrdersToExport()
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                using (var tx = conn.BeginTransaction())
                using (var cmd = new SqlCommand("UPDATE OrdersProcessed SET BatchId = @BatchId", conn, tx))
                {
                    cmd.Parameters.AddWithValue("BatchId", Data.BatchId);
                    cmd.ExecuteNonQuery();

                    tx.Commit();
                }
            }
        }

        private IEnumerable<string> GetOrdersProcessed()
        {
            Console.WriteLine("Retrieving orders...");

            var items = new List<string>();

            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                using (var tx = conn.BeginTransaction())
                using (var cmd = new SqlCommand("SELECT OrderId, PurchaseDate, Amount FROM OrdersProcessed WHERE BatchId = @BatchId", conn, tx))
                {
                    cmd.Parameters.AddWithValue("BatchId", Data.BatchId);

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

    }
}