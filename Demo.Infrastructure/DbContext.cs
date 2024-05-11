using Demo.Infrastructure;
using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;

namespace Demo.Infrastructure
{
    public class DbContext : IDbContext
    {
        public string ConnectionString { get; set; }
        public DbConnection Connection { get { return CreateConnection(); } set { conn = value; } }

        public DbContext(string connectionString)
        {
            ConnectionString = connectionString;
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
