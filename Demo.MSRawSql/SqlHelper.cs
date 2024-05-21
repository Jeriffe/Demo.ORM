using Demo.Infrastructure;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Data.Common;

namespace Demo.MSRawSql
{
    public static class SqlHelper
    {

        public static object ExecuteRawScalar(this DbConnection conn, DbTransaction trans, string sql, params RawParameter[] parameters)
        {
            conn.EnsureOpenConn();
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = sql;
                cmd.Transaction = trans;

                if (parameters != null && parameters.Length > 0)
                {
                    cmd.Parameters.Clear();
                    foreach (var p in parameters)
                    {
                        cmd.Parameters.Add(new SqlParameter(p.Name, p.Value));
                    }
                }
                var result = cmd.ExecuteScalar();

                return result;
            }
        }

        public static void ExecuteNoQueryRawSql(this DbConnection conn, DbTransaction trans, string sql, params RawParameter[] parameters)
        {
            conn.EnsureOpenConn();
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = sql;
                cmd.Transaction = trans;

                if (parameters != null && parameters.Length > 0)
                {
                    cmd.Parameters.Clear();
                    foreach (var p in parameters)
                    {
                        cmd.Parameters.Add(new SqlParameter(p.Name, p.Value));
                    }
                }

                cmd.ExecuteNonQuery();
            }
        }

        public static DataTable ExecuteRawSql(this DbConnection conn, DbTransaction trans, string sql, params RawParameter[] parameters)
        {
            conn.EnsureOpenConn();
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = sql;
                cmd.Transaction = trans;

                if (parameters != null && parameters.Length > 0)
                {
                    cmd.Parameters.Clear();
                    foreach (var p in parameters)
                    {
                        cmd.Parameters.Add(new SqlParameter(p.Name, p.Value));
                    }
                }

                using (var dataReader = cmd.ExecuteReader())
                {
                    if (dataReader.HasRows)
                    {
                        var dataTable = new DataTable();
                        dataTable.Load(dataReader);
                        return dataTable;
                    }
                }

                return null;

            }
        }
    }
}
