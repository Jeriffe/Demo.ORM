using Demo.Infrastructure;
using Microsoft.IdentityModel.Tokens;
using RepoDb;
using RepoDb.Enumerations;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;

namespace Demo.RepoDBConsole.Repositories
{
    public interface IRepoDBRepository<TEntity, TKey> : IRepository<TEntity, TKey>
        where TEntity : class, new()
        where TKey : struct
    {
        TEntity GetSingle(Expression<Func<TEntity, bool>> expr);

        IEnumerable<TEntity> GetList(Expression<Func<TEntity, bool>> expr = null);

        IEnumerable<TEntity> GetList(PageFilterWithOrderBy pagFilter, Expression<Func<TEntity, bool>> expr = null);

        void Create(IEnumerable<TEntity> entities);

        void Update(IEnumerable<TEntity> entities);

        void Delete(IEnumerable<TEntity> entities);
    }

    public class GenericRepository<TEntity> : IRepoDBRepository<TEntity, int> where TEntity : class, new()
    {
        public string ConnectionString { get; }

        private const int BatchNumber = 100;

        private IUnitOfWork<IDbContext> unitOfWork;

        public IDbContext Context { get { return unitOfWork.Context; } }

        public GenericRepository(IUnitOfWork unitOfWork)
        {
            if (!(unitOfWork is IUnitOfWork<IDbContext>))
            {
                throw new ArgumentException("Expected IUnitOfWork<DapperDbContext>");
            }

            this.unitOfWork = unitOfWork as IUnitOfWork<IDbContext>;
        }

        public IUnitOfWork UnitOfWork
        {
            get { return unitOfWork; }
        }

        public TEntity GetSingle(Expression<Func<TEntity, bool>> expr)
        {
            return Context.Connection.Query(expr, transaction: unitOfWork.Transaction).FirstOrDefault();
        }

        public IEnumerable<TEntity> GetList(Expression<Func<TEntity, bool>> expr = null)
        {
            return Context.Connection.Query(expr, transaction: unitOfWork.Transaction);
        }

        public IEnumerable<TEntity> GetList(PageFilterWithOrderBy pagFilter, Expression<Func<TEntity, bool>> expr = null)
        {
            var orderFields = new List<OrderField> { new OrderField(pagFilter.OrderBy, GetOrder(pagFilter.OrderSorting)) };

            return Context.Connection.BatchQuery(pagFilter.PagIndex, pagFilter.PageSize, orderFields, expr, transaction: unitOfWork.Transaction);
        }

        private Order GetOrder(OrderSorting orderSorting)
        {
            switch (orderSorting)
            {
                case OrderSorting.DESC:
                    return Order.Descending;
                default:
                case OrderSorting.ASC:
                    return Order.Ascending;
            }
        }

        public virtual IEnumerable<TEntity> GetAll(string cacheKey = null)
        {
            return Context.Connection.QueryAll<TEntity>(cacheKey: cacheKey, transaction: unitOfWork.Transaction);
        }


        public virtual int Create(TEntity entity)
        {
            return Context.Connection.Insert<TEntity, int>(entity, transaction: unitOfWork.Transaction);
        }

        public virtual void Update(TEntity entity)
        {
            Context.Connection.Update<TEntity>(entity, transaction: unitOfWork.Transaction);
        }
        public virtual void Delete(TEntity entity)
        {
            Context.Connection.Delete<TEntity>(entity, transaction: unitOfWork.Transaction);
        }

        /* Batch Operations */



        public virtual void Create(IEnumerable<TEntity> entities)
        {
            var isBatch = entities.Count() >= BatchNumber;
            var rows = default(int);
            if (isBatch)
            {
                rows = Context.Connection.InsertAll<TEntity>(entities, batchSize: BatchNumber, transaction: unitOfWork.Transaction);
            }
            else
            {
                foreach (var entity in entities)
                {
                    Context.Connection.Insert(entity, transaction: unitOfWork.Transaction);
                    rows++;
                }
            }
        }

        public virtual void Update(IEnumerable<TEntity> entities)
        {
            var isBatch = entities.Count() >= BatchNumber;
            var rows = default(int);
            if (isBatch)
            {
                rows = Context.Connection.UpdateAll<TEntity>(entities, transaction: unitOfWork.Transaction);
            }
            else
            {
                foreach (var entity in entities)
                {
                    rows += Context.Connection.Update(entity, transaction: unitOfWork.Transaction);
                }
            }

        }

        public virtual void Delete(IEnumerable<TEntity> entities)
        {
            var isBatch = entities.Count() >= BatchNumber;
            var rows = default(int);
            if (isBatch)
            {
                rows = Context.Connection.DeleteAll<TEntity>(entities, transaction: unitOfWork.Transaction);
            }
            else
            {
                foreach (var entity in entities)
                {
                    rows += Context.Connection.Delete(entity, transaction: unitOfWork.Transaction);
                }
            }
        }

        public TEntity GetByKey(int id)
        {
            return Context.Connection.Query<TEntity>(id, transaction: unitOfWork.Transaction).FirstOrDefault();
        }

        public TEntity GetSingle(string sql, CommandType commandType = CommandType.Text, object anonymousConditions = null)
        {
            var result = Context.Connection.ExecuteQuery<TEntity>(sql, anonymousConditions, commandType: commandType, transaction: unitOfWork.Transaction).FirstOrDefault();

            return result;
        }

        public IEnumerable<TEntity> GetList(string sql, CommandType commandType = CommandType.Text, object anonymousConditions = null)
        {
            var result = Context.Connection.ExecuteQuery<TEntity>(sql, anonymousConditions, commandType: commandType, transaction: unitOfWork.Transaction);

            return result;
        }

        protected int GetOffset(int pageIndex, int pageSize)
        {
            return pageIndex * pageSize;
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    if (unitOfWork != null)
                    {
                        unitOfWork.Dispose();
                        unitOfWork = null;
                    }
                }

                disposed = true;
            }
        }
    }
}
