using MySql.Data.MySqlClient;
using System;
using System.Reflection;
using System.Text;
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
                //await ReadAllPatients(conn, SqlGetPcnt);
                await GetPatientResearches(conn);
                //await conn.CloseAsync();
            }
        }
        private static async Task ReadAllPatients(MySqlConnection conn,string sqlRequest)
        {
            // Retrieve all rows
            var i = 0;
            using (var cmd = new MySqlCommand(sqlRequest, conn))
            using (var reader = await cmd.ExecuteReaderAsync())
            {
                try
                {
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
                            Console.WriteLine($"{f.Name.Trim('<').Remove(f.Name.Length - 17)}: {f.GetValue(patient)}");
                        }
                        
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    await conn.CloseAsync();
                }
            }
                
            
           
        }
        private static async Task GetPatientResearches(MySqlConnection conn/*, Patient patient, Guid guid*/)
        {
            var i = 0;
            StringBuilder sqlRequest = new StringBuilder();
            sqlRequest
                .Append("SELECT ")
                //.Append("PCNT.PCNT_ID_GUID AS 'PCNT_ID_GUID',")
                //.Append("CONCAT_WS(' ', PCNT.LASTNAME, PCNT.FIRSTNAME) AS 'LASTNAME',")
                //.Append("RESEARCHVALUE.RESVALUE AS 'RESVALUE',")
                //.Append("RESEARCHVALUE.NAME AS 'RES_NAME',")
                //.Append("RESEARCHVALUE.DESCRIPTION AS 'DESCRIPTION'")
                .Append("* ")
                .Append("FROM PCNT ")
                .Append("LEFT JOIN RESEARCHES ON ")
                .Append("(PCNT.PCNT_ID_GUID = RESEARCHES.PCNT_ID_GUID) ")
                .Append("LEFT JOIN DATAINFO ON ")
                .Append("(RESEARCHES.RESEARCH_ID_GUID = DATAINFO.RESEARCH_ID_GUID) ")
                .Append("LEFT JOIN RESEARCHVALUE ON ")
                .Append("(DATAINFO.DATAINFO_ID_GUID = RESEARCHVALUE.DATAINFO_ID_GUID) ")
                .Append("WHERE ")
                .Append("PCNT.PCNT_ID_GUID = 'b482872a-cc50-472e-9b23-a0ffa039f75f'");
            using (var cmd = new MySqlCommand(sqlRequest.ToString(), conn))
            using (var reader = await cmd.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    Console.WriteLine($"\r");
                    Console.WriteLine($"Pcntname: {reader.GetString(1)}, ResName:{reader.GetString(28)}, ResValue:{reader.GetString(29)}, Description:{reader.GetString(30)}");
                }   
            }
        }
    }
}
