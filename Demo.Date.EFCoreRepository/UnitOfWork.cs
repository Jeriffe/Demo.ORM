using Demo.Infrastructure;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Demo.Date.EFCoreRepository
{
    public class UnitOfWork : IUnitOfWork<IDbContext>
    {
        public IRawSqlExecutor SqlExecutor { get; private set; }

        public UnitOfWork(IDbContext context, IRawSqlExecutor sqlExecutor = null)
        {
            SqlExecutor = sqlExecutor;

            if (SqlExecutor == null)
            {
                SqlExecutor = new DefaultEFCoreRawExecutor((SqlDBcontext)context);
            }

            Context = context;
        }
        public IDbContext Context { get; private set; }

        public IDbTransaction Transaction => throw new NotImplementedException();

        public void BeginTrans()
        {
            throw new NotImplementedException();
        }

        public void Commit()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void ExecuteRawNoQuery(string sql, CommandType commandType = CommandType.Text, params RawParameter[] parameters)
        {
            throw new NotImplementedException();
        }

        public object ExecuteRawScalar(string sql, CommandType commandType = CommandType.Text, params RawParameter[] parameters)
        {
            throw new NotImplementedException();
        }

        public DataTable ExecuteRawSql(string sql, CommandType commandType = CommandType.Text, params RawParameter[] parameters)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> ExecuteRawSql<T>(string sql, CommandType commandType = CommandType.Text, params RawParameter[] parameters) where T : class, new()
        {
            return SqlExecutor.ExecuteRawSql<T>(null, null,sql,parameters: parameters);
        }

        public IEnumerable<T> ExecuteRawSql<T>(string sql, CommandType commandType = CommandType.Text, params SqlParameter[] parameters) where T : class, new()
        {
            return (SqlExecutor as DefaultEFCoreRawExecutor).ExecuteRawSql<T>(null, null, sql, parameters: parameters);
        }

        public void ExecuteRawNoQuery<T>(string sql, CommandType commandType = CommandType.Text, params SqlParameter[] parameters) where T : class, new()
        {
            (SqlExecutor as DefaultEFCoreRawExecutor).ExecuteNoQueryRawSql<T>(null, null, sql, parameters: parameters);
        }



        public void Rollback()
        {
            throw new NotImplementedException();
        }
    }
}
