using MySql.Data.MySqlClient;
using System;

 

namespace alpha69.common
{
    public class DBAccess
    {
        private string DB_SERVER;
        private int DB_PORT;
        private string DB_USERNAME;
        private string DB_PASSWORD;
        private string DB_DATABASE;
        private string DB_SSLMODE;

      

        public MySqlConnection Connection;

        public DBAccess()
        {
            this.DB_SERVER = System.Environment.GetEnvironmentVariable("DB_SERVER");
            this.DB_PORT = int.Parse(System.Environment.GetEnvironmentVariable("DB_PORT"));
            this.DB_USERNAME = System.Environment.GetEnvironmentVariable("DB_USERNAME");
            this.DB_PASSWORD = System.Environment.GetEnvironmentVariable("DB_PASSWORD");
            this.DB_DATABASE = System.Environment.GetEnvironmentVariable("DB_DATABASE");
            this.DB_SSLMODE = System.Environment.GetEnvironmentVariable("DB_SSLMODE");
       

            this.Connection=new MySqlConnection(string.Format("server={0};uid={1};pwd={2};database={3};port={4};SslMode={5}",
                this.DB_SERVER,
                this.DB_USERNAME,
                this.DB_PASSWORD,
                this.DB_DATABASE,
                this.DB_PORT,
                this.DB_SSLMODE));
        }

        public DBAccess(string dbServer, string dbUsername, string dbPassword, string dbDatabase, int dbPort, string sslMode)
        {

            this.DB_SERVER = dbServer;
            this.DB_PORT = dbPort;
            this.DB_USERNAME = dbUsername;
            this.DB_PASSWORD = dbPassword;
            this.DB_DATABASE = dbDatabase;
            this.DB_SSLMODE = sslMode;
  
            this.Connection = new MySqlConnection(string.Format("server={0};uid={1};pwd={2};database={3};port={4};SslMode={5}",
                this.DB_SERVER,
                this.DB_USERNAME,
                this.DB_PASSWORD,
                this.DB_DATABASE,
                this.DB_PORT,
                this.DB_SSLMODE));
        }

        public string Test()
        {
            try
            {
                this.Connection.Open();
                this.Connection.Close();
                return "ok";
            }
            catch (Exception ex)
            {
                return $"failed: {ex.Message}";
            }
        }


    }
}
