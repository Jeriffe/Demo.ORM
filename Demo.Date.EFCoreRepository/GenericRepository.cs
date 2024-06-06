using Demo.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Demo.Date.EFCoreRepository
{
    public class GenericRepository<TEntity> : IRepository<TEntity>
        where TEntity : class, new()
    {
        public GenericRepository(IUnitOfWork unitOfWork)
        {
            if (unitOfWork is null)
            {
                throw new ArgumentNullException("unitOfWork");
            }

            if (!(unitOfWork is IUnitOfWork<IDbContext>))
            {
                throw new ArgumentException("Expected IUnitOfWork<IDbContext from EFCore>");
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
            DBContext.SaveChanges();
            return model.Entity;
        }

        public void Delete(TEntity item)
        {
            DBContext.Remove(item);
            DBContext.SaveChanges();

        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public TEntity Get(Expression<Func<TEntity, bool>> predicateExpr)
        {
            return DBContext.Set<TEntity>().Where(predicateExpr).FirstOrDefault();

        }

        public TEntity GetByKey(object id)
        {
            return DBContext.Find<TEntity>(id);
        }

        public IEnumerable<TEntity> GetList(Expression<Func<TEntity, bool>> predicateExpr = null)
        {
            if (predicateExpr == null)
            {
                return DBContext.Set<TEntity>();
            }
            return DBContext.Set<TEntity>().Where(predicateExpr);
        }

        public void Update(TEntity item)
        {
            DBContext.Entry(item).State = EntityState.Modified;
            DBContext.SaveChanges();
        }



        public void BulkCreate(IEnumerable<TEntity> entities)
        {
            DBContext.BulkInsert(entities,
                options =>
                {
                    options.BatchSize = 2000;
                    options.BatchTimeout = 180;
                });
        }

        public void BulkUpdate(IEnumerable<TEntity> entities)
        {
            DBContext.BulkUpdate(entities,
                options =>
                {
                    options.BatchSize = 2000;
                    options.BatchTimeout = 180;
                });
        }

        public void BulkDelete(IEnumerable<TEntity> entities)
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
