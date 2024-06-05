using Demo.Infrastructure;
using Microsoft.Data.SqlClient;
using RepoDb;
using System.Data;
using System.Data.Common;

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

            GlobalConfiguration.Setup().UseSqlServer();
        }


        private DbConnection conn;
        public DbConnection CreateConnection()
        {
            if (conn == null)
            {
                conn = new SqlConnection(ConnectionString);
            }

            return conn;
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
