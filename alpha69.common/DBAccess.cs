using System;
using MySql.Data.MySqlClient;

namespace alpha69.common
{
    public class DBAccess
    {
        public MySqlConnection Connection;
        private readonly string DB_DATABASE;
        private readonly string DB_PASSWORD;
        private readonly int DB_PORT;
        private readonly string DB_SERVER;
        private readonly string DB_SSLMODE;
        private readonly string DB_USERNAME;

        public DBAccess()
        {
            DB_SERVER = Environment.GetEnvironmentVariable("DB_SERVER");
            DB_PORT = int.Parse(Environment.GetEnvironmentVariable("DB_PORT"));
            DB_USERNAME = Environment.GetEnvironmentVariable("DB_USERNAME");
            DB_PASSWORD = Environment.GetEnvironmentVariable("DB_PASSWORD");
            DB_DATABASE = Environment.GetEnvironmentVariable("DB_DATABASE");
            DB_SSLMODE = Environment.GetEnvironmentVariable("DB_SSLMODE");


            Connection = new MySqlConnection(string.Format(
                "server={0};uid={1};pwd={2};database={3};port={4};SslMode={5}",
                DB_SERVER,
                DB_USERNAME,
                DB_PASSWORD,
                DB_DATABASE,
                DB_PORT,
                DB_SSLMODE));
        }

        public DBAccess(string dbServer, string dbUsername, string dbPassword, string dbDatabase, int dbPort,
            string sslMode)
        {
            DB_SERVER = dbServer;
            DB_PORT = dbPort;
            DB_USERNAME = dbUsername;
            DB_PASSWORD = dbPassword;
            DB_DATABASE = dbDatabase;
            DB_SSLMODE = sslMode;

            Connection = new MySqlConnection(string.Format(
                "server={0};uid={1};pwd={2};database={3};port={4};SslMode={5}",
                DB_SERVER,
                DB_USERNAME,
                DB_PASSWORD,
                DB_DATABASE,
                DB_PORT,
                DB_SSLMODE));
        }

        public string Test(string message = "ping")
        {
            try
            {
                Connection.Open();
                var cmd = new MySqlCommand("SELECT now()", Connection);
                cmd.ExecuteNonQuery();
                Connection.Close();
                return message;
            }
            catch (Exception ex)
            {
                return $"failed: {ex.Message}";
            }
        }
    }
}