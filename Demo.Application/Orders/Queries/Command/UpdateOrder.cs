using AutoMapper;
using Demo.Data.Models;
using Demo.DTOs;
using Demo.Infrastructure;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Application.Orders.Command
{
    public class UpdateOrderCommand : IRequest
    {
        public Order Order { get; set; }
    }

    public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand>
    {
        private IMapper mapper;
        private IRepository<TOrder> orderRepository;
        private IRepository<TOrderItem> orderItemRepository;

        public UpdateOrderCommandHandler(IRepository<TOrder> orderRepository, IRepository<TOrderItem> orderItemRepository, IMapper mapper)
        {
            this.orderRepository = orderRepository;
            this.orderItemRepository = orderItemRepository;
            this.mapper = mapper;
        }

        public Task<Unit> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            var model = mapper.Map<TOrder>(request.Order);

            var dbOrderItems = orderItemRepository.GetList(o => o.OrderId == request.Order.Id);

            var updateOrderItems = dbOrderItems.Intersect(model.OrderItems, new OrderItemComparer());
            var newOrderItems = model.OrderItems.Except(dbOrderItems, new OrderItemComparer());
            var deleteOrderItems = dbOrderItems.Except(updateOrderItems, new OrderItemComparer());

            orderItemRepository.BulkUpdate(updateOrderItems);

            newOrderItems.ToList().ForEach(o => o.OrderId = model.Id);
            orderItemRepository.BulkCreate(newOrderItems);

            orderItemRepository.BulkDelete(deleteOrderItems);

            orderRepository.Update(model);


            return Task.FromResult(Unit.Value);
        }
    }


    public class OrderItemComparer : IEqualityComparer<TOrderItem>
    {
        public bool Equals(TOrderItem x, TOrderItem y)
        {
            return x.Id == y.Id;
        }
        public int GetHashCode(TOrderItem obj)
        {
            return obj.Id.GetHashCode() ^ obj.GetHashCode();
        }
    }
}
