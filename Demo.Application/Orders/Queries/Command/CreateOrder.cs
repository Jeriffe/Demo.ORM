using AutoMapper;
using Demo.Data.Models;
using Demo.DTOs;
using Demo.Infrastructure;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Application.Orders.Command
{
    public class CreateOrderCommand : IRequest<Order>
    {
        public Order Order { get; set; }
    }

    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Order>
    {
        private IMapper mapper;
        private IRepository<TOrder> orderRepository;
        private IRepository<TOrderItem> orderItemRepository;

        public CreateOrderCommandHandler(IRepository<TOrder> orderRepository, IRepository<TOrderItem> orderItemRepository, IMapper mapper)
        {
            this.orderRepository = orderRepository;
            this.orderItemRepository = orderItemRepository;
            this.mapper = mapper;
        }

        public async Task<Order> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var model = mapper.Map<TOrder>(request.Order);

            var dbModel = orderRepository.Create(model);

            model.OrderItems.ToList().ForEach(o=> { o.OrderId=dbModel.Id; });

            orderItemRepository.BulkCreate(model.OrderItems);

            var dto = mapper.Map<Order>(dbModel);

            return dto;
        }
    }
}
