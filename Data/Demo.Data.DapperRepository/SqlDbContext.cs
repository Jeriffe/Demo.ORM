using Demo.Infrastructure;
using Microsoft.Data.Sqlite;
using MySql.Data.MySqlClient;
using Npgsql;
using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace Demo.Data.DapperRepository
{
    public class SqlDbContext : Infrastructure.IDbContext
    {
        public string ConnectionString { get; set; }
        public DataProviderType ProviderName { get; set; }
        public DbConnection Connection { get { return CreateConnection(); } set { conn = value; } }

        static SqlDbContext()
        {
            FluentMappers.Initialize();
            DapperPlusMapper.Initialize();
        }

        public SqlDbContext(string connectionString, DataProviderType providerName = DataProviderType.SQLServer)
        {
            //ProviderName = "SQL Server";
            ProviderName = providerName;

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
                case DataProviderType.Sqlite:

                    /*Storage Class	Meaning
                    NULL	    values mean missing information or unknown.
                    INTEGER	    values are whole numbers (either positive or negative). 
                    REAL	    values are real numbers with decimal values that use 8-byte floats.
                    TEXT	    is used to store character data. The maximum length of TEXT is unlimited. 
                    BLOB	    stands for a binary large object that can store any kind of data. the maximum size of BLOB is unlimited.

                    INTEGER	
                           INT
                           INTEGER
                           TINYINT
                           SMALLINT
                           MEDIUMINT
                           BIGINT
                           UNSIGNED BIG INT
                           INT2
                           INT8	
                    TEXT
                           CHARACTER(20)
                           VARCHAR(255)
                           VARYING CHARACTER(255)
                           NCHAR(55)
                           NATIVE CHARACTER(70)
                           NVARCHAR(100)
                           TEXT
                           CLOB		
                   BLOB
                           BLOB
                   REAL
                           DOUBLE
                           DOUBLE PRECISION
                           FLOAT
                   NUMERIC
                           DECIMAL(10,5)
                           BOOLEAN
                           DATE
                           DATETIME
                        */

                    return new SqliteConnection(ConnectionString);
                //NuGet\Install-Package Npgsql -Version 8.0.3
                case DataProviderType.PostgreSQL:

                    return new NpgsqlConnection(ConnectionString);
                case DataProviderType.MySQL:
                    //NuGet\Install-Package MySql.Data -Version 8.4.0
                    return new MySqlConnection(ConnectionString);
                case DataProviderType.SQLServer:

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
