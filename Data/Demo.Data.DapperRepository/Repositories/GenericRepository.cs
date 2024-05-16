using Dapper;
using Demo.Infrastructure;
using Dommel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;

namespace Demo.Data.DapperRepository
{
    public class GenericRepository<TEntity> : IRepository<TEntity> where TEntity : class, new()
    {
        private IUnitOfWork<IDbContext> unitOfWork;

        public IDbContext Context { get { return unitOfWork.Context; } }

        public GenericRepository(IUnitOfWork unitOfWork)
        {
            if (!(unitOfWork is IUnitOfWork<IDbContext>))
            {
                throw new ArgumentException("Expected IUnitOfWork<Microsoft.EntityFrameworkCore.DbContext>");
            }

            this.unitOfWork = unitOfWork as IUnitOfWork<IDbContext>;
        }

        #region IRepository<TEntity>
        public IUnitOfWork UnitOfWork
        {
            get { return unitOfWork; }
        }


        public bool Any(Expression<Func<TEntity, bool>> predicateExpr)
        {
            return GetQuery().Any(predicateExpr);
        }

        public int Count(Expression<Func<TEntity, bool>> predicateExpr)
        {
            return GetQuery().Count(predicateExpr);

        }
        public IEnumerable<TEntity> Get()
        {
            return GetQuery().ToList();
        }

        public IEnumerable<TEntity> GetList(Expression<Func<TEntity, bool>> predicateExpr)
        {
            if (predicateExpr == null)
            {
                return GetQuery().ToList();
            }

            return GetQuery().Where(predicateExpr);
        }

        public TEntity GetByKey(int Id)
        {
            return Context.Connection.Get<TEntity>(Id);
        }


        public TEntity Get(Expression<Func<TEntity, bool>> predicateExpr)
        {
            return GetQuery().FirstOrDefault(predicateExpr);
        }


        private IQueryable<TEntity> GetQuery()
        {

            return Context.Connection.GetAll<TEntity>().AsQueryable();
        }

        public virtual TEntity Create(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            var obj = Context.Connection.Insert(entity);

            var id=Convert.ToInt32(obj);

            return GetByKey(id);
        }

  
        public void Delete(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            Context.Connection.Delete(entity);
        }

        public void Delete(Expression<Func<TEntity, bool>> criteria)
        {
            var records = GetList(criteria);

            foreach (var r in records)
            {
                Context.Connection.Delete(r);
            }
        }

        public void Update(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            Context.Connection.Update(entity);
        }

        #endregion

        public TEntity GetSingle(string sql, CommandType commandType = CommandType.Text, object whereConditions = null)
        {
            return Context.Connection.QueryFirst<TEntity>(sql, commandType: commandType);

        }

        public IEnumerable<TEntity> GetList(string sql, CommandType commandType = CommandType.Text, object parameters = null)
        {
            return Context.Connection.Query<TEntity>(sql, commandType: commandType);
        }
      

        #region IDisposable

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

        #endregion
    }
}
