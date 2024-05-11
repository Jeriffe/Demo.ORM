using System;
using System.Collections.Generic;
using System.Data;
using System.Transactions;

namespace Demo.Infrastructure
{
    public interface IUnitOfWork : IDisposable
    {

        void SaveChanges();

        void BeginTrans();

        void Commit();

        void Rollback();


        void ExecuteRawSql(string sql, object parameters = null);
        /// <summary>
        /// Executes the query, and returns the first column of the first row in the result set returned by the query. 
        /// The additional columns or rows are ignored.
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        object Executescalar(string sql, object parameter = null);
    }

    public interface IUnitOfWork<T> : IUnitOfWork where T : class
    {
        T Context { get; }

        IDbTransaction Transaction{ get; }
    }



}