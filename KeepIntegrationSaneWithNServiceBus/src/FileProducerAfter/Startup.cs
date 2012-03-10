using System;
using System.Data.SqlClient;
using FileProducerAfterMessages;
using NServiceBus;

namespace FileProducerAfter
{
    public class Startup : IWantToRunAtStartup
    {
        private static readonly string _connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=KeepingIntegrationsSane;Integrated Security=SSPI;";

        public IBus Bus { get; set; }

        public void Run()
        {
            while (true)
            {
                ResetData();

                Console.WriteLine("Press Enter to begin...");
                Console.ReadLine();
                Bus.SendLocal(new ExportFile {BatchId = Guid.NewGuid()});
            }
        }

        public void Stop()
        {
        }

        private static void ResetData()
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                using (var tx = conn.BeginTransaction())
                using (var cmd = new SqlCommand("UPDATE OrdersProcessed SET BatchId = NULL", conn, tx))
                {
                    cmd.ExecuteNonQuery();

                    tx.Commit();
                }
            }
        }

    }
}