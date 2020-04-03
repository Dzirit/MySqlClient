using MySql.Data.MySqlClient;
using System;
using System.Threading.Tasks;

namespace MySqlClient
{
    class Program
    {
        const string SqlGetPcnt = "SELECT * FROM pcnt";
        static async Task Main(string[] args)
        {
            Console.WriteLine("Openning Connection ...");
            await ConnectDB();
            Console.WriteLine("Done.");

        }
        private static async Task ConnectDB()
        {
            string connStr = "server=localhost;user=PCNTDB;database=start_db;port=3309;password=123456";

            using (var conn = new MySqlConnection(connStr))
            {
                await conn.OpenAsync();
                Console.WriteLine($"Connection is {conn.State}");
                await ReadTable(conn, SqlGetPcnt);
            }

        }
        private static async Task ReadTable(MySqlConnection conn,string sqlRequest)
        {
            // Retrieve all rows
            using (var cmd = new MySqlCommand(sqlRequest, conn))
            using (var reader = await cmd.ExecuteReaderAsync())
                while (await reader.ReadAsync())
                    Console.WriteLine($"{reader[1]}");
            await conn.CloseAsync();

        }
    }
}
