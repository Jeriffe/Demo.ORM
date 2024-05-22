using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Demo.Infrastructure
{
    public interface IUnitOfWork : IDisposable
    {
        void BeginTrans();

        void Commit();

        void Rollback();


        object ExecuteRawScalar(string sql, CommandType commandType = CommandType.Text, params RawParameter[] parameters);

        void ExecuteRawNoQuery( string sql, CommandType commandType = CommandType.Text, params RawParameter[] parameters);

        DataTable ExecuteRawSql( string sql, CommandType commandType = CommandType.Text, params RawParameter[] parameters);

        IEnumerable<T> ExecuteRawSql<T>(string sql, CommandType commandType = CommandType.Text, params RawParameter[] parameters)
            where T : class, new();
    }

    public interface IUnitOfWork<T> : IUnitOfWork
    {
        T Context { get; }

        IDbTransaction Transaction { get; }
    }

    public class RawParameter
    {
        public string Name { get; set; }
        public object Value { get; set; }
    }


}