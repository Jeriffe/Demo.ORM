using Demo.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Demo.Date.EFCoreRepository
{
    public class GenericRepository<TEntity> : IRepository<TEntity>
        where TEntity : class, new()
    {
        public GenericRepository(IUnitOfWork unitOfWork)
        {
            if (!(unitOfWork is IUnitOfWork<IDbContext>))
            {
                throw new ArgumentException("Expected IUnitOfWork<IDbContext from RepoDb>");
            }

            this.unitOfWork = unitOfWork as IUnitOfWork<IDbContext>;

            if (this.unitOfWork.Context is SqlDBcontext)
            {
                DBContext = (this.unitOfWork.Context as SqlDBcontext);
            }
        }
        public DbContext DBContext;
        public IUnitOfWork UnitOfWork
        {
            get { return unitOfWork; }
        }
        private IUnitOfWork<IDbContext> unitOfWork;


        public TEntity Create(TEntity item)
        {
            var model = DBContext.Add(item);

            return model.Entity;
        }

        public void Delete(TEntity item)
        {
            DBContext.Remove(item);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public TEntity Get(Expression<Func<TEntity, bool>> predicateExpr)
        {
            throw new NotImplementedException();
        }

        public TEntity GetByKey(object id)
        {
            return DBContext.Find<TEntity>(id);
        }

        public IEnumerable<TEntity> GetList(Expression<Func<TEntity, bool>> predicateExpr = null)
        {
            throw new NotImplementedException();
        }

        public void Update(TEntity item)
        {
            throw new NotImplementedException();
        }



        public void Create(IEnumerable<TEntity> entities)
        {
            DBContext.BulkInsert(entities, 
                options => 
                { 
                    options.BatchSize = 2000;
                    options.BatchTimeout = 180;
                });
        }

        public void Update(IEnumerable<TEntity> entities)
        {
            DBContext.BulkUpdate(entities,
                options =>
                {
                    options.BatchSize = 2000;
                    options.BatchTimeout = 180;
                });
        }

        public void Delete(IEnumerable<TEntity> entities)
        {
            DBContext.BulkDelete(entities,
                options =>
                {
                    options.BatchSize = 2000;
                    options.BatchTimeout = 180;
                });
        }
    }
}
