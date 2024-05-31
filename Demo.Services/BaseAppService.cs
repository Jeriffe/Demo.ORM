using AutoMapper;
using Demo.DTOs;
using Demo.Infrastructure;
using Microsoft.Extensions.Logging;
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
        protected ILogger<IAppService<TDTO>>   logger;

        protected IRepository<TEntity> entityRepository;
        protected IUnitOfWork unitOfWork;
        protected IMapper mapper;
        public BaseAppService(IUnitOfWork uow, IRepository<TEntity> repository, IMapper mapper, ILogger<IAppService<TDTO>>  logger=null)
        {
            unitOfWork = uow;
            entityRepository = repository;
            this.mapper = mapper;
            this.logger=logger;
        }

        public virtual TDTO GetSingle(object keyId)
        {
            logger?.LogInformation($"Start {logger.GetType().Name}-->.BaseAppService.GetSingle......");

            var entity = entityRepository.GetByKey(keyId);

            var dto = mapper.Map<TDTO>(entity);

            return dto;
        }
        public IEnumerable<TDTO> GetAll(PageFilter pageFilter)
        {
            logger?.LogInformation($"Start {logger.GetType().Name}-->.BaseAppService.GetAll......");

            var models = entityRepository.GetList();

            var dtos = mapper.Map<List<TDTO>>(models);
            return dtos;
        }

        public virtual TDTO Create(TDTO item)
        {
            logger?.LogInformation($"Start {logger.GetType().Name}-->.BaseAppService.Create......");
            var model = mapper.Map<TEntity>(item);

            var dbModel = entityRepository.Create(model);

            var dto = mapper.Map<TDTO>(dbModel);

            return dto;
        }

        public virtual void Delete(TDTO item)
        {
            logger?.LogInformation($"Start {logger.GetType().Name}-->.BaseAppService.Delete......");
            var model = mapper.Map<TEntity>(item);

            entityRepository.Delete(model);

        }

        public virtual void Update(TDTO item)
        {
            logger?.LogInformation($"Start {logger.GetType().Name}-->.BaseAppService.Update......");
            var model = mapper.Map<TEntity>(item);

            entityRepository.Update(model);

        }


    }
}
