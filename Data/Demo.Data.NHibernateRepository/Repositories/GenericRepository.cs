﻿using Demo.Infrastructure;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using static System.Collections.Specialized.BitVector32;

namespace Demo.Data.NHibernateRepository
{
    public class GenericRepository<TEntity> : IRepository<TEntity> where TEntity : class, new()
    {
        private IUnitOfWork<IDbContext> unitOfWork;

        public NHibernate.ISession Session { get; private set; }

        public GenericRepository(IUnitOfWork unitOfWork)
        {
            if (!(unitOfWork is IUnitOfWork<IDbContext>))
            {
                throw new ArgumentException("Expected IUnitOfWork<Microsoft.EntityFrameworkCore.DbContext>");
            }

            this.unitOfWork = unitOfWork as IUnitOfWork<IDbContext>;

            if (this.unitOfWork.Context is SqlDbContext)
            {
                Session = (this.unitOfWork.Context as SqlDbContext).Session;
            }
        }

        #region IRepository<TEntity>
        public IUnitOfWork UnitOfWork
        {
            get { return unitOfWork; }
        }

        public void Trans(Action action)
        {
            //Fix:NHibernate.NonUniqueObjectException: 'a different object with the same identifier value was already associated
            Session.Clear();
            using (ITransaction trans = Session.BeginTransaction())
            {
                action();

                trans.Commit();
            }

        }
        public T Trans<T>(Func<T> action)
        {
            //Fix:NHibernate.NonUniqueObjectException: 'a different object with the same identifier value was already associated
            Session.Clear();
            using (ITransaction transaction = Session.BeginTransaction())
            {
                var result = action();

                transaction.Commit();

                return result;
            }

        }

        public bool Any(Expression<Func<TEntity, bool>> predicateExpr)
        {
            return Session.Query<TEntity>().Any(predicateExpr);
        }

        public int Count(Expression<Func<TEntity, bool>> predicateExpr)
        {
            return Session.Query<TEntity>().Count(predicateExpr);

        }
        public IEnumerable<TEntity> Get()
        {

            return Session.Query<TEntity>().ToList();
        }

        public IEnumerable<TEntity> GetList(Expression<Func<TEntity, bool>> predicateExpr)
        {
            if (predicateExpr == null)
            {
                return Session.Query<TEntity>().ToList();
            }

            return Session.Query<TEntity>().Where(predicateExpr).ToList();
        }

        public TEntity GetByKey(int Id)
        {
            return Session.Get<TEntity>(Id);
        }


        public TEntity Get(Expression<Func<TEntity, bool>> predicateExpr)
        {
            var result = Session.Query<TEntity>().Where(predicateExpr).ToList();
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

            return Trans(() =>
            {
                var obj = Session.Save(entity);
                var id = Convert.ToInt32(obj);
                var item = GetByKey(id);

                return item;
            }
           );


        }


        public void Delete(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            Session.Clear();
            Trans(() => Session.Delete(entity));

        }

        public void Delete(Expression<Func<TEntity, bool>> criteria)
        {
            var records = GetList(criteria);

            Trans(() =>
            {                
                foreach (var r in records)
                {
                    Session.Delete(r);
                }
            });
        }

        public void Update(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            Trans(() =>
            {
                Session.Merge(entity);
            });
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