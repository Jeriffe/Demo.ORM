﻿using Demo.Infrastructure;
using Demo.RawSql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Demo.Data.NHibernateRepository
{
    public class UnitOfWork : IUnitOfWork<IDbContext>
    {
        private IDbTransaction trans = null;


        public IDbTransaction Transaction => trans;

        public IDbContext Context { get; private set; }

        public IRawSqlExecutor SqlExecutor { get; private set; }

        public UnitOfWork(IDbContext context, IRawSqlExecutor sqlExecutor = null)
        {
            SqlExecutor = sqlExecutor;

            if (SqlExecutor == null)
            {
                SqlExecutor = new DapperRawExecutor();
            }

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
            catch (Exception) { throw; }
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
                //Context.Dispose();
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
    }

}

