using Demo.Infrastructure;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Text;

namespace Demo.Date.EFCoreRepository
{
    public class UnitOfWork : IUnitOfWork<IDbContext>
    {
        private IDbTransaction trans;

        public IRawSqlExecutor SqlExecutor { get; private set; }
        public IDbContext Context { get; private set; }
        public IDbTransaction Transaction => trans;

        public UnitOfWork(IDbContext context, IRawSqlExecutor sqlExecutor = null)
        {
            SqlExecutor = sqlExecutor;

            if (SqlExecutor == null)
            {
                SqlExecutor = new DefaultEFCoreRawExecutor((EFCoreDBcontext)context);
            }

            Context = context;
        }

        public void BeginTrans()
        {
            BeginTransaction(IsolationLevel.ReadCommitted);
        }


        public void BeginTransaction(IsolationLevel isolationLevel)
        {
            if (trans != null)
            {
                throw new Exception("CANNOT_BEGIN_NEW_TRANSACTION_WHILE_A_TRANSACTION_IS_RUNNING");
            }

            trans = Context.CreateConnection().BeginTransaction(isolationLevel);
        }


        public void Commit()
        {
            if (trans == null)
            {
                throw new Exception("CANNOT_COMMIT_TRANSACTION_WHILE_NO_TRANSACTION_IS_RUNNING");
            }

            try
            {

                trans.Commit();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                DisposeTransaction();
            }
        }
        private void DisposeTransaction()
        {
            if (trans != null)
            {
                trans.Dispose();
                trans = null;
            }

            Context.CloseConnection();
        }
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public object ExecuteRawScalar(string sql, CommandType commandType = CommandType.Text, params RawParameter[] parameters)
        {
            var result = SqlExecutor.ExecuteRawScalar(Context.Connection, trans as DbTransaction, sql, commandType, parameters);

            return result;
        }

        public void ExecuteRawNoQuery(string sql, CommandType commandType = CommandType.Text, params RawParameter[] parameters)
        {
            SqlExecutor.ExecuteNoQueryRawSql(Context.Connection, trans as DbTransaction, sql, commandType, parameters);

        }

        public DataTable ExecuteRawSql(string sql, CommandType commandType = CommandType.Text, params RawParameter[] parameters)
        {
            return SqlExecutor.ExecuteRawSql(Context.Connection, trans as DbTransaction, sql, commandType, parameters);
        }

        public IEnumerable<T> ExecuteRawSql<T>(string sql, CommandType commandType = CommandType.Text, params RawParameter[] parameters) where T : class, new()
        {
            return SqlExecutor.ExecuteRawSql<T>(Context.Connection, trans as DbTransaction, sql, commandType, parameters);
        }

        public void Rollback()
        {
            if (trans == null)
            {
                throw new Exception("CANNOT_ROLLBACK_TRANSACTION_WHILE_NO_TRANSACTION_IS_RUNNING");
            }

            try
            {
                trans.Rollback();
            }
            finally
            {
                DisposeTransaction();
            }
        }
    }
}
