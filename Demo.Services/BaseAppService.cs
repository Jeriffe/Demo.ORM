using Demo.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace Demo.Services
{
    public interface IAppService<TEntity> where TEntity : class, new()
    {
        TEntity GetSingle(int keyId);

        TEntity Create(TEntity item);

        void Update(TEntity item);

        void Delete(TEntity item);
    }

    public abstract class BaseAppService<TEntity> : IAppService<TEntity> where TEntity : class, new()
    {
        protected IRepository<TEntity> entityRepository;
        protected IUnitOfWork unitOfWork;
        public BaseAppService(IUnitOfWork uow, IRepository<TEntity> repository)
        {
            unitOfWork = uow;
            entityRepository = repository;
        }
        public TEntity GetSingle(int keyId)
        {
          var entity= entityRepository.GetByKey(keyId);

            return entity;
        }

        public TEntity Create(TEntity item)
        {
           return entityRepository.Create(item);
        }

        public void Delete(TEntity item)
        {
            entityRepository.Delete(item);

        }

        public void Update(TEntity item)
        {
            entityRepository.Update(item);

        }
    }
}
