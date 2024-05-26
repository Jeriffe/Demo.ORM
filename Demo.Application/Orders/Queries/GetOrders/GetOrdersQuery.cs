using AutoMapper;
using Demo.Data.Models;
using Demo.DTOs;
using Demo.Infrastructure;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Application
{
    public class GetOrdersQuery : IRequest<List<Order>>
    {
        public PageFilter PageFilter { get; set; }
    }

    public class GetOrdersQueryHandler : IRequestHandler<GetOrdersQuery, List<Order>>
    {
        private IMapper mapper;
        private IUnitOfWork unitOfWork;
        private IRepository<TOrder> entityRepository;
    
        public GetOrdersQueryHandler(IUnitOfWork uow, IRepository<TOrder> repository, IMapper mapper)
        {
            unitOfWork = uow;
            entityRepository = repository;
            this.mapper = mapper;
        }

        public async Task<List<Order>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
        {
            var models = entityRepository.GetList();

            var dtos = mapper.Map<List<Order>>(models);

            return dtos;
        }
    }
}
