﻿using Demo.Data.DapperRepository.Mappers;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace Demo.Data.DapperRepository
{
    public class SqlDbContext : Infrastructure.IDbContext
    {
        public string ConnectionString { get; set; }
        public DbConnection Connection { get { return CreateConnection(); } set { conn = value; } }

        static SqlDbContext()
        {
            FluentMappers.Initialize();
        }

        public SqlDbContext(string connectionString)
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