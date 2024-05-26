using Demo.Infrastructure;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Demo.Data.NHibernateRepository
{
    public class GenericRepository<TEntity> : IRepository<TEntity>
       where TEntity : class, new()
    {
        private IUnitOfWork<IDbContext> unitOfWork;

        public ISession Session { get; private set; }

        public GenericRepository(IUnitOfWork unitOfWork)
        {
            if (!(unitOfWork is IUnitOfWork<IDbContext>))
            {
                throw new ArgumentException("Expected IUnitOfWork<NHibernate.ISession>");
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

        public TEntity GetByKey(object id)
        {
            return Session.Get<TEntity>(id);
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
            });
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

        public void Create(IEnumerable<TEntity> entities)
        {
            if (entities == null)
            {
                throw new ArgumentNullException("entity");
            }

            if (entities.Count() == 0)
            {
                return;
            }

            //Trans(() =>
            //{
            //    //https://nhibernate.info/doc/nhibernate-reference/batch.html

            //    var list = entities.ToList();

            //    for (int index = 0; index < list.Count; index++)
            //    {
            //        var obj = Session.Save(list[index]);

            //        var item = GetByKey(obj);

            //        // 20, same as the ADO batch size
            //        if (index % 20 == 0)
            //        {
            //            // flush a batch of inserts and release memory:
            //            Session.Flush();
            //            Session.Clear();
            //        }
            //    }
            //});

            TransStateless((IStatelessSession session) =>
            {
                foreach (var entity in entities)
                {
                    session.Insert(entity);
                }

            });

        }

        public void Update(IEnumerable<TEntity> entities)
        {
            TransStateless((IStatelessSession session) =>
            {
                foreach (var entity in entities)
                {
                    session.Update(entity);
                }

            });
        }

        public void Delete(IEnumerable<TEntity> entities)
        {
            TransStateless((IStatelessSession session) =>
            {
                foreach (var entity in entities)
                {
                    session.Delete(entity);
                }

            });
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
        private   void TransStateless(Action<IStatelessSession> action)
        {
            //https://nhibernate.info/blog/2008/10/30/bulk-data-operations-with-nhibernate-s-stateless-sessions.html

            using (IStatelessSession statelessSession = SqlDbContext.SessionFactory.OpenStatelessSession())
            using (ITransaction trans = statelessSession.BeginTransaction())
            {
                action(statelessSession);

                trans.Commit();
            }
        }

        #endregion
    }
}

