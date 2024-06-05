using Demo.Infrastructure;
using RepoDb;
using RepoDb.Enumerations;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Demo.Data.RepoDBRepository
{
    public class GenericRepository<TEntity> : IRepository<TEntity>
        where TEntity : class, new()
    {
        public string ConnectionString { get; }

        private const int BatchNumber = 1000;

        private IUnitOfWork<IDbContext> unitOfWork;

        public IDbContext Context { get { return unitOfWork.Context; } }

        public GenericRepository(IUnitOfWork unitOfWork)
        {
            if (!(unitOfWork is IUnitOfWork<IDbContext>))
            {
                throw new ArgumentException("Expected IUnitOfWork<IDbContext from RepoDb>");
            }

            this.unitOfWork = unitOfWork as IUnitOfWork<IDbContext>;
        }

        public IUnitOfWork UnitOfWork
        {
            get { return unitOfWork; }
        }

        public TEntity Get(Expression<Func<TEntity, bool>> expr)
        {
            return Context.Connection.Query(expr, transaction: unitOfWork.Transaction).FirstOrDefault();
        }

        public IEnumerable<TEntity> GetList(Expression<Func<TEntity, bool>> predicateExpr = null)
        {
            if (predicateExpr == null)
            {
                return Context.Connection.Query(predicateExpr, transaction: unitOfWork.Transaction).ToList();
            }

            return Context.Connection.Query(predicateExpr, transaction: unitOfWork.Transaction);
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

        public virtual TEntity Create(TEntity entity)
        {
            var id = Context.Connection.Insert<TEntity, int>(entity, transaction: unitOfWork.Transaction);

            return GetByKey(id);
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


        public virtual void BulkCreate(IEnumerable<TEntity> entities)
        {
            var rows = default(int);
            //You can adjust the size of your batch by simply passing the value at the batchSize argument.By default, the value is 10
            rows = Context.Connection.InsertAll<TEntity>(entities, batchSize: BatchNumber, transaction: unitOfWork.Transaction);
        }

        public virtual void BulkUpdate(IEnumerable<TEntity> entities)
        {
            var rows = default(int);

            rows = Context.Connection.UpdateAll<TEntity>(entities, transaction: unitOfWork.Transaction);

        }

        public virtual void BulkDelete(IEnumerable<TEntity> entities)
        {
            var rows = default(int);

            rows = Context.Connection.DeleteAll<TEntity>(entities, transaction: unitOfWork.Transaction);
        }

        public TEntity GetByKey(object id)
        {
            return Context.Connection.Query<TEntity>(id, transaction: unitOfWork.Transaction).FirstOrDefault();
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
