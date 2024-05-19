using AutoMapper;
using Demo.Data.Models;
using Demo.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace Demo.Services
{
    public interface IAppService<TEntity, TDTO> where TEntity : class, new()
        where TDTO : class, new()
    {
        IEnumerable<TDTO> GetAll(PageFilter pageFilter);

        TDTO GetSingle(int keyId);

        TDTO Create(TDTO item);

        void Update(TDTO item);

        void Delete(TDTO item);
    }

    public abstract class BaseAppService<TEntity, TDTO> : IAppService<TEntity, TDTO>
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

        public TDTO GetSingle(int keyId)
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

        public TDTO Create(TDTO item)
        {
            var model = mapper.Map<TEntity>(item);

            var dbModel = entityRepository.Create(model);

            var dto = mapper.Map<TDTO>(dbModel);

            return dto;
        }

        public void Delete(TDTO item)
        {
            var model = mapper.Map<TEntity>(item);

            entityRepository.Delete(model);

        }

        public void Update(TDTO item)
        {
            var model = mapper.Map<TEntity>(item);

            entityRepository.Update(model);

        }


    }
}
