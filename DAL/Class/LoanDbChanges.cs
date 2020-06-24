using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace DAL.Class
{
    public class LoanDbChanges
    {

        public string AppVersion { get; set; }

        public async void Configurate()
        {
            try
            {
                AppVersion = (await DLicense.GetData())[0].AppVersion ?? "0.0.0.0";
            }
            catch
            {
                AppVersion = "0.0.0.0";
            }
            if (string.CompareOrdinal("1.1.0", AppVersion) <= 0) return;

            const string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;Database=dbLoan;Integrated Security=True;Connect Timeout=30";
            var cn = new SqlConnection(connectionString);
            try
            {
                //Execute DB Script'
                var scriptFileName = Directory.GetCurrentDirectory() + @"\LocalDBChangesScript.sql";
                var reader = new StreamReader(scriptFileName);
                var cmd = reader.ReadToEnd();
                cmd = cmd.Replace("\r\n", " ");
                var commands = cmd.Split('ƒ');
                var appVersion = "0.0.0.0";
                cn.Open();
                foreach (var command in commands)
                {
                    switch (command)
                    {
                        case "/*AppVersion 1.1.0*/":
                            appVersion = "1.1.0";
                            continue;
                    }
                    if (string.CompareOrdinal(appVersion, AppVersion) <= 0) continue;
                    var cm = new SqlCommand(command, cn);
                    cm.ExecuteNonQuery();
                }
            }
            catch
            {
                // ignored
            }
            finally
            {
                if (cn.State != ConnectionState.Closed)
                    cn.Close();
                cn.Dispose();
            }
        }
    }
}
