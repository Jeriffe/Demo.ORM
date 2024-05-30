using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Demo.Infrastructure
{
    public interface IRepository<TEntity> : IDisposable
        where TEntity : class, new()
    {
        IUnitOfWork UnitOfWork { get; }

        TEntity GetByKey(object id);

        TEntity Get(Expression<Func<TEntity, bool>> predicateExpr);

        IEnumerable<TEntity> GetList(Expression<Func<TEntity, bool>> predicateExpr = null);

        TEntity Create(TEntity item);

        void Update(TEntity item);

        void Delete(TEntity item);


        void BulkCreate(IEnumerable<TEntity> entities);

        void BulkUpdate(IEnumerable<TEntity> entities);

        void BulkDelete(IEnumerable<TEntity> entities);
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
