using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;

namespace Demo.Infrastructure
{
    public interface IRepository<TEntity> : IRepository<TEntity, int>
         where TEntity : class, new()
    {
    }

    public interface IRepository<TEntity, TKey> : IDisposable
        where TEntity : class, new()
        where TKey : struct
    {
        IUnitOfWork UnitOfWork { get; }

        TEntity GetByKey(TKey id);

        TEntity Get(Expression<Func<TEntity, bool>> predicateExpr);

        IEnumerable<TEntity> GetList(Expression<Func<TEntity, bool>> predicateExpr = null);

        TEntity GetSingle(string sql, CommandType commandType = CommandType.Text, object whereConditions = null);
        IEnumerable<TEntity> GetList(string sql, CommandType commandType = CommandType.Text, object parameters = null);


        TEntity Create(TEntity item);

        void Update(TEntity item);

        void Delete(TEntity item);
    }
    public enum OrderSorting
    {
        ASC,
        DESC
    }
    public class PageFilterWithOrderBy : PageFilter
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
