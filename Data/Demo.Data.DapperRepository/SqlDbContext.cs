using Microsoft.Data.Sqlite;
using Npgsql;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace Demo.Data.DapperRepository
{
    public class SqlDbContext : Infrastructure.IDbContext
    {
        public string ConnectionString { get; set; }
        public string ProviderName { get; set; }
        public DbConnection Connection { get { return CreateConnection(); } set { conn = value; } }

        static SqlDbContext()
        {
            FluentMappers.Initialize();
            DapperPlusMapper.Initialize();
        }

        public SqlDbContext(string connectionString,string providerName= "SQL Server")
        {
            //ProviderName = "SQL Server";
            ProviderName = providerName ;

            ConnectionString = connectionString;

            ConnectionString = @"Data Source=D:\\Jeriffe\\Examples\\C#\\git\\Demo.ORM\\0_DB\ORM_DEMO.db;";
        }

        private DbConnection conn;
        public DbConnection CreateConnection()
        {

            if (conn == null)
            {
                conn = DbConnnectionFactory();
            }

            return conn;
        }

        private DbConnection DbConnnectionFactory()
        {
            switch (ProviderName)
            {
                //NuGet\Install-Package Microsoft.Data.Sqlite -Version 8.0.6
                case "SQLite":
                 

                    return new SqliteConnection(ConnectionString);
                //NuGet\Install-Package Npgsql -Version 8.0.3
                case "PostgreSQL ":
                    return new NpgsqlConnection(ConnectionString);
                //case "MySQL":
                //    break;
                case "SQL Server":
                default:
                    return new SqlConnection(ConnectionString);
            }

        }

        public void CloseConnection()
        {
            if (conn != null)
            {
                if (conn.State != ConnectionState.Closed)
                {
                    conn.Close();
                }
            }
        }

        public void Dispose()
        {
            CloseConnection();

            Connection = null;
        }
    }
}
