using Demo.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace Demo.RawSql
{
    public class DataRawSqlExecutor : IRawSqlExecutor
    {
        public object ExecuteRawScalar(DbConnection conn, DbTransaction trans, string sql, CommandType commandType = CommandType.Text, params RawParameter[] parameters)
        {
            conn.EnsureOpenConn();
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = sql;
                cmd.CommandType = commandType;
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

        public void ExecuteNoQueryRawSql(DbConnection conn, DbTransaction trans, string sql, CommandType commandType = CommandType.Text, params RawParameter[] parameters)
        {
            conn.EnsureOpenConn();
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = sql;
                cmd.CommandType = commandType;
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

        public DataTable ExecuteRawSql(DbConnection conn, DbTransaction trans, string sql, CommandType commandType = CommandType.Text, params RawParameter[] parameters)
        {
            conn.EnsureOpenConn();
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = sql;
                cmd.CommandType = commandType;
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

        public IEnumerable<T> ExecuteRawSql<T>(DbConnection conn, DbTransaction trans, string sql, CommandType commandType = CommandType.Text, params RawParameter[] parameters) where T : class, new()
        {
            var dt = ExecuteRawSql(conn, trans, sql, commandType, parameters);

            //c# reflect to map DataTable to T object
            throw new NotImplementedException("c# reflect to map DataTable to T object");
        }
    }
}
