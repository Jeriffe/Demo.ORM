using System;
using System.Collections.Generic;
using System.Data;

namespace Demo.Infrastructure
{
    public interface IDapperSimpleCRUD<TEntity> : IDisposable where TEntity : class, new()
    {
        TEntity GetSingle(object whereConditions = null);
        /// <summary>
        ///  With parameters:  conn.GetList<User>(new { Age = 10 });  
        ///  With where clause:conn.GetList<User>("where age = 10 or Name like '%Smith%'");  
        /// </summary>
        /// <param name="whereConditions"></param>
        /// <returns></returns>
        IEnumerable<TEntity> GetList(object whereConditions = null);
        /// <summary>
        /// With parameters:   conn.GetListPaged<User>(1,10,"where age = @Age","Name desc", new {Age = 10});  
        /// With where clause: conn.GetListPaged<User>(1,10,"where age = 10 or Name like '%Smith%'","Name desc"); 
        /// </summary>
        /// <param name="pageindex"></param>
        /// <param name="pageSize"></param>
        /// <param name="conditions"></param>
        /// <param name="orderby"></param>
        /// <param name="whereConditions"></param>
        /// <returns></returns>
        IEnumerable<TEntity> GetListPaged(int pageindex, int pageSize, string conditions, string orderby, object whereConditions = null);

    }

    public interface IRepository<TEntity>: IDisposable where TEntity : class, new()
    {
        IUnitOfWork UnitOfWork { get; }

        TEntity GetByKey(int id);

        TEntity GetSingle(string sql, CommandType commandType = CommandType.Text, object whereConditions = null);
        IEnumerable<TEntity> GetList(string sql, CommandType commandType = CommandType.Text, object parameters = null);

        int Create(TEntity item);

        void Update(TEntity item);

        void Delete(TEntity item);
    }


    public enum OrderSorting
    {
        ASC,
        DESC
    }
    public class PageFilterWithOrderBy: PageFilter
    {
        public string OrderBy { get; set; }

        public OrderSorting OrderSorting { get; set; }
    }
    public class PageFilter
    {
        public int PagIndex { get; set; }
        public int PageSize { get; set; }
    }
}
