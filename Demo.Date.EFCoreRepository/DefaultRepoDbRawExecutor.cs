﻿using Demo.Infrastructure;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Demo.Date.EFCoreRepository
{
    internal class DefaultEFCoreRawExecutor : IRawSqlExecutor
    {
        private readonly SqlDBcontext dbContext;

        public DefaultEFCoreRawExecutor(SqlDBcontext dbContext)
        {
            this.dbContext = dbContext;
        }
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
            return dbContext.Set<T>().FromSqlRaw(string.Format(sql, parameters));
        }

        public IEnumerable<T> ExecuteRawSql<T>(DbConnection conn, DbTransaction trans, string sql, CommandType commandType = CommandType.Text, params SqlParameter[] parameters) where T : class, new()
        {
            return dbContext.Set<T>().FromSqlRaw(sql, parameters);
        }

        public void ExecuteNoQueryRawSql<T>(DbConnection conn, DbTransaction trans, string sql, CommandType commandType = CommandType.Text, params SqlParameter[] parameters) where T : class, new()
        {
            dbContext.Database.ExecuteSqlRaw(sql, parameters);

        }
    }
}