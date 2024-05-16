using Demo.Infrastructure;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;

namespace Demo.Data.NHibernateRepository
{
    public class GenericRepository<TEntity> : IRepository<TEntity, int> where TEntity : class, new()
    {
        private IUnitOfWork<NHibernate.ISession> unitOfWork;

        public NHibernate.ISession Context { get { return unitOfWork.Context; } }

        public GenericRepository(IUnitOfWork unitOfWork)
        {
            if (!(unitOfWork is IUnitOfWork<NHibernate.ISession>))
            {
                throw new ArgumentException("Expected IUnitOfWork<Microsoft.EntityFrameworkCore.DbContext>");
            }

            this.unitOfWork = unitOfWork as IUnitOfWork<NHibernate.ISession>;
        }

        #region IRepository<TEntity>
        public IUnitOfWork UnitOfWork
        {
            get { return unitOfWork; }
        }

        public void Trans(Action action)
        {
            using (ITransaction transaction = Context.BeginTransaction())
            {
                action();

                transaction.Commit();
            }

        }
        public T Trans<T>(Func<T> action)
        {
            using (ITransaction transaction = Context.BeginTransaction())
            {
                var result = action();

                transaction.Commit();

                return result;
            }

        }
        
        public bool Any(Expression<Func<TEntity, bool>> predicateExpr)
        {
            return Context.Query<TEntity>().Any(predicateExpr);
        }

        public int Count(Expression<Func<TEntity, bool>> predicateExpr)
        {
            return Context.Query<TEntity>().Count(predicateExpr);

        }
        public IEnumerable<TEntity> Get()
        {

            return Context.Query<TEntity>().ToList();
        }

        public IEnumerable<TEntity> GetList(Expression<Func<TEntity, bool>> predicateExpr)
        {
            return Context.Query<TEntity>().Where(predicateExpr).ToList();
        }

        public TEntity GetByKey(int Id)
        {
            return Context.Get<TEntity>(Id);
        }


        public TEntity Get(Expression<Func<TEntity, bool>> predicateExpr)
        {
            var result = Context.Query<TEntity>().Where(predicateExpr).ToList();
            if (result != null & result.Count > 0)
            {
                return result[0];
            }

            return null;
        }


        public virtual TEntity Create(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            return Trans<TEntity>(() =>
            {
                Context.SaveOrUpdate(entity);

                //var id = Convert.ToInt32(obj);
                return null;
            }
           );


        }


        public void Delete(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            Trans(() => Context.Delete(entity));

        }

        public void Delete(Expression<Func<TEntity, bool>> criteria)
        {
            var records = GetList(criteria);

            Trans(() => 
            { 
                foreach (var r in records)
                {
                     Context.Delete(r);
                }
            });
}

        public void Update(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            Trans(() => Context.Update(entity));
        }

        #endregion

        public TEntity GetSingle(string sql, CommandType commandType = CommandType.Text, object whereConditions = null)
        {
            //return Context.Connection.QueryFirst<TEntity>(sql, commandType: commandType);
            return null;

        }

        public IEnumerable<TEntity> GetList(string sql, CommandType commandType = CommandType.Text, object parameters = null)
        {
            //return Context.Connection.Query<TEntity>(sql, commandType: commandType);
            return null;
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
