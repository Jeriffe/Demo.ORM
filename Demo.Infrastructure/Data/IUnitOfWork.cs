using System;
using System.Data;

namespace Demo.Infrastructure
{
    public interface IUnitOfWork : IDisposable
    {

        void SaveChanges();

        void BeginTrans();

        void Commit();

        void Rollback();


        void ExecuteNoQueryRawSql(string sql, object parameters = null);
        DataTable ExecuteRawSql(string sql, object parameter = null);

        /// <summary>
        /// Executes the query, and returns the first column of the first row in the result set returned by the query. 
        /// The additional columns or rows are ignored.
        /// EG:returns the new Identity column value if a new row was inserted, 0 on failure.
        ///     INSERT INTO Production.ProductCategory (Name) VALUES (@Name); SELECT CAST(scope_identity() AS int)
        /// EG:{cmd.CommandText = "SELECT COUNT(1) FROM dbo.region"; var count = (int)cmd.ExecuteScalar();}
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        object ExecuteScalar(string sql, object parameter = null);
      
    }

    public interface IUnitOfWork<T> : IUnitOfWork where T : class
    {
        T Context { get; }

        IDbTransaction Transaction{ get; }
    }



}