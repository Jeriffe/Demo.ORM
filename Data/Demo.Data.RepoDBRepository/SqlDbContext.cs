using Demo.Infrastructure;
using Microsoft.Data.SqlClient;
using MySql.Data.MySqlClient;
using Npgsql;
using RepoDb;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;

namespace Demo.Data.RepoDBRepository
{
    public class SqlDbContext : Infrastructure.IDbContext
    {
        public string ConnectionString { get; set; }
        public DbConnection Connection { get { return CreateConnection(); } set { conn = value; } }
        public DataProviderType ProviderName { get; set; }

        static SqlDbContext()
        {
            FluentMappers.Initialize();
        }

        public SqlDbContext(string connectionString, DataProviderType providerName = DataProviderType.SQLServer)
        {
            ConnectionString = connectionString;

            ProviderName = providerName;
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
                //NuGet\Install-Package RepoDb.SQLite.System -Version 1.13.1
                case DataProviderType.Sqlite:
                    GlobalConfiguration.Setup().UseSQLite();

                    return new SQLiteConnection(ConnectionString);

                //NuGet\Install-Package RepoDb.PostgreSql -Version 1.13.1
                case DataProviderType.PostgreSQL:
                    GlobalConfiguration.Setup().UsePostgreSql();

                    return new NpgsqlConnection(ConnectionString);

                //NuGet\Install-Package RepoDb.MySql -Version 1.13.1
                case DataProviderType.MySQL:
                    GlobalConfiguration.Setup().UseMySql();

                    return new MySqlConnection(ConnectionString);

                //NuGet\Install-Package RepoDb.SqlServer -Version 1.13.1
                case DataProviderType.SQLServer:

                default:

                    GlobalConfiguration.Setup().UseSqlServer();
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
