using MySql.Data.MySqlClient;
using System;
using System.Reflection;
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
                await ReadAllPatients(conn, SqlGetPcnt);
            }

        }
        private static async Task ReadAllPatients(MySqlConnection conn,string sqlRequest)
        {
            // Retrieve all rows
            var i = 0;
            using (var cmd = new MySqlCommand(sqlRequest, conn))
            using (var reader = await cmd.ExecuteReaderAsync())
                while (await reader.ReadAsync())
                {
                    Console.WriteLine($"\r");
                    i++;
                    Console.WriteLine($"Patient №{i} ");
                    Patient patient = new Patient();
                    patient.PublicIdGuid = reader.GetGuid(0);
                    patient.FirstName = reader.GetString(1);
                    patient.LastName = reader.GetString(2);
                    FieldInfo[] fields = patient.GetType().GetFields(BindingFlags.Public |
                                          BindingFlags.NonPublic |
                                          BindingFlags.Instance);
                    foreach (var f in fields)
                    {
                        Console.WriteLine($"{f.Name.Trim('<').Remove(f.Name.Length-17)}: {f.GetValue(patient)}");
                    }                   
                }
            await conn.CloseAsync();
        }
        private static async Task GetPatientResearches(MySqlConnection conn, Patient patient)
        {
            var sqlRequest= "SELECT 
                            RESEARCHES.RESEARCH_ID_GUID AS 'GUID', 
                            DATE_FORMAT(RESEARCHES.DATEOFRESEARCH, '%d.%m.%Y') AS 'DATE',
                            RESEARCHES.RESEARCHNAME AS 'NAME', 
                            RESEARCHES.DOCTOR AS 'DOCTOR', 
                            RESEARCHES.DEPARTMENT AS 'DEPARTMENT', 
                            RESEARCHES.INSTITUTE AS 'INSTITUTE'
                            FROM
                            RESEARCHES
                            LEFT JOIN DATAINFO ON
                            DATAINFO.RESEARCH_ID_GUID = RESEARCHES.RESEARCH_ID_GUID
                            WHERE
                            PCNT_ID_GUID = '5d3292db-ac18-4459-9b32-5c654aa26064'
                            GROUP BY RESEARCHES.RESEARCH_ID_GUID"
        }
    }
}
