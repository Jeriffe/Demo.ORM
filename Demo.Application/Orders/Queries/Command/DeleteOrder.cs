using AutoMapper;
using Demo.Data.Models;
using Demo.Infrastructure;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Application.Orders.Command
{
    public class DeleteOrderCommand : IRequest
    {
        public int Id { get; set; }
    }

    public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand>
    {
        private IMapper mapper;
        private IRepository<TOrder> orderRepository;
        private IRepository<TOrderItem> orderItemRepository;

        public DeleteOrderCommandHandler(IRepository<TOrder> orderRepository, IRepository<TOrderItem> orderItemRepository, IMapper mapper)
        {
            this.orderRepository = orderRepository;
            this.orderItemRepository = orderItemRepository;
            this.mapper = mapper;
        }

        public Task<Unit> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {
            var model = orderRepository.GetByKey(request.Id);

            var orderItems = orderItemRepository.GetList(o => o.OrderId == model.Id);

            orderItemRepository.BulkDelete(orderItems);

            orderRepository.Delete(model);

            return Task.FromResult(Unit.Value);
        }
    }
}
