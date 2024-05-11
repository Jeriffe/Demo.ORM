using System;
using System.Collections.Generic;
using System.Data;

namespace Demo.Infrastructure
{
    public interface IRepository<TEntity, TKey> : IDisposable 
        where TEntity : class, new() 
        where TKey : struct
    {
        IUnitOfWork UnitOfWork { get; }

        TEntity GetByKey(TKey id);

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
