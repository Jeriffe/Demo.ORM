using Dapper;
using Demo.Infrastructure;
using System;
using System.Data;
using System.Data.Common;
using Demo.RawSql;

namespace Demo.Data.DapperRepository
{
    public class UnitOfWork : IUnitOfWork<IDbContext>
    {
        private IDbTransaction trans = null;

        public IDbContext Context { get; private set; }

        public IDbTransaction Transaction => trans;

        public UnitOfWork(IDbContext context)
        {
            Context = context;
        }

        public void BeginTrans()
        {
            if (trans == null)
            {
                var conn = Context.CreateConnection();
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }

                trans = conn.BeginTransaction();
            }
        }

        public void SaveChanges()
        {

        }

        public void Commit()
        {
            if (trans == null)
            {
                throw new Exception("CANNOT_COMMIT_TRANSACTION_WHILE_NO_TRANSACTION");
            }

            try
            {
                trans.Commit();
            }
            catch
            {
                throw;
            }
            finally
            {
                DisposeTransaction();
            }
        }

        public void Rollback()
        {
            if (trans != null)
            {
                trans.Rollback();
            }
        }

        public void Dispose()
        {
            if (Context != null)
            {
                Context.Dispose();
                Context = null;
            }
        }

        private void DisposeTransaction()
        {
            if (trans != null)
            {
                trans.Dispose();

                trans = null;
            }

            CloseConnection();
        }

        private void CloseConnection()
        {
            Context.CloseConnection();
        }

        public object ExecuteRawScalar(string sql, params RawParameter[] parameters)
        {


            var result = Context.Connection.ExecuteRawScalar(trans as DbTransaction, sql, parameters);

            return result;
        }

        public void ExecuteRawNoQuery(string sql, params RawParameter[] parameters)
        {
            Context.Connection.ExecuteRawSql(trans as DbTransaction, sql, parameters);

        }

        public DataTable ExecuteRawSql(string sql, params RawParameter[] parameters)
        {
            return Context.Connection.ExecuteRawSql(trans as DbTransaction, sql, parameters);
        }
    }

}

