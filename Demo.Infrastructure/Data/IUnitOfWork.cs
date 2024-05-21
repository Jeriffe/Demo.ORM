using System;
using System.Data;

namespace Demo.Infrastructure
{
    public interface IUnitOfWork : IDisposable
    {
        void BeginTrans();

        void Commit();

        void Rollback();


        void ExecuteRawNoQuery(string sql, params RawParameter[] parameters);
        DataTable ExecuteRawSql(string sql, params RawParameter[] parameter);

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
        object ExecuteRawScalar(string sql, params RawParameter[] parameter);
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