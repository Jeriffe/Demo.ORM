using AutoMapper;
using Demo.Infrastructure;
using System.Collections.Generic;

namespace Demo.Services
{
    public interface IAppService<TDTO>
        where TDTO : class, new()
    {
        IEnumerable<TDTO> GetAll(PageFilter pageFilter);

        TDTO GetSingle(object keyId);

        TDTO Create(TDTO item);

        void Update(TDTO item);

        void Delete(TDTO item);
    }

    public abstract class BaseAppService<TEntity, TDTO> : IAppService<TDTO>
        where TEntity : class, new()
        where TDTO : class, new()

    {
        protected IRepository<TEntity> entityRepository;
        protected IUnitOfWork unitOfWork;
        protected IMapper mapper;
        public BaseAppService(IUnitOfWork uow, IRepository<TEntity> repository, IMapper mapper)
        {
            unitOfWork = uow;
            entityRepository = repository;
            this.mapper = mapper;
        }

        public virtual TDTO GetSingle(object keyId)
        {
            var entity = entityRepository.GetByKey(keyId);

            var dto = mapper.Map<TDTO>(entity);

            return dto;
        }
        public IEnumerable<TDTO> GetAll(PageFilter pageFilter)
        {
            var models = entityRepository.GetList();

            var dtos = mapper.Map<List<TDTO>>(models);
            return dtos;
        }

        public virtual TDTO Create(TDTO item)
        {
            var model = mapper.Map<TEntity>(item);

            var dbModel = entityRepository.Create(model);

            var dto = mapper.Map<TDTO>(dbModel);

            return dto;
        }

        public virtual void Delete(TDTO item)
        {
            var model = mapper.Map<TEntity>(item);

            entityRepository.Delete(model);

        }

        public virtual void Update(TDTO item)
        {
            var model = mapper.Map<TEntity>(item);

            entityRepository.Update(model);

        }


    }
}
