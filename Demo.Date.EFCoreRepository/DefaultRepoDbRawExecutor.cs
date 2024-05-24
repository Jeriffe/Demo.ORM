using Demo.Infrastructure;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Demo.Date.EFCoreRepository
{
    internal class DefaultRepoDEFcORERawExecutor : IRawSqlExecutor
    {
        public void ExecuteNoQueryRawSql(DbConnection conn, DbTransaction trans, string sql, CommandType commandType = CommandType.Text, params RawParameter[] parameters)
        {
            throw new System.NotImplementedException();
        }

        public object ExecuteRawScalar(DbConnection conn, DbTransaction trans, string sql, CommandType commandType = CommandType.Text, params RawParameter[] parameters)
        {
            throw new System.NotImplementedException();
        }

        public DataTable ExecuteRawSql(DbConnection conn, DbTransaction trans, string sql, CommandType commandType = CommandType.Text, params RawParameter[] parameters)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<T> ExecuteRawSql<T>(DbConnection conn, DbTransaction trans, string sql, CommandType commandType = CommandType.Text, params RawParameter[] parameters) where T : class, new()
        {
            throw new System.NotImplementedException();
        }
    }
}