using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Demo.Infrastructure
{
    public interface IRawSqlExecutor
    {
        object ExecuteRawScalar(DbConnection conn, DbTransaction trans, string sql, CommandType commandType=CommandType.Text, params RawParameter[] parameters);

        void ExecuteNoQueryRawSql(DbConnection conn, DbTransaction trans, string sql, CommandType commandType = CommandType.Text, params RawParameter[] parameters);

        DataTable ExecuteRawSql(DbConnection conn, DbTransaction trans, string sql, CommandType commandType = CommandType.Text, params RawParameter[] parameters);

        IEnumerable<T> ExecuteRawSql<T>(DbConnection conn, DbTransaction trans, string sql, CommandType commandType = CommandType.Text, params RawParameter[] parameters)
            where T : class, new();
    }
}
