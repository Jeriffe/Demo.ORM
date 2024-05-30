using AutoMapper;
using Demo.Data.Models;
using Demo.DTOs;
using Demo.Infrastructure;
using System.Collections.Generic;
using System.Linq;

namespace Demo.Services
{
    public interface IOrderSvc : IAppService<Order>
    {
    }

    public class OrderSvc : BaseAppService<TOrder, Order>, IOrderSvc
    {
        protected IRepository<TOrderItem> orderItemRepository;

        public OrderSvc(IUnitOfWork unitOfWork, IRepository<TOrder> repository, IRepository<TOrderItem> orderItemrepository, IMapper mapper)
            : base(unitOfWork, repository, mapper)
        {
            this.orderItemRepository = orderItemrepository;
        }

        public override Order GetSingle(object keyId)
        {
            var order = base.GetSingle(keyId);

            var dbOrderItems = orderItemRepository.GetList(o => o.OrderId == order.Id);

            order.OrderItems = mapper.Map<List<OrderItem>>(dbOrderItems);

            return order;
        }

        public override Order Create(Order item)
        {
            var order = unitOfWork.ProcessWithTrans(() =>
             {
                 var dbModel = base.Create(item);

                 item.OrderItems.ToList().ForEach(o => { o.OrderId = dbModel.Id; });

                 var orderItems = mapper.Map<List<TOrderItem>>(item.OrderItems);

                 orderItemRepository.BulkCreate(orderItems);

                 var result = mapper.Map<Order>(dbModel);

                 return result;

             });

            return order;
        }

        public override void Update(Order item)
        {
            unitOfWork.ProcessWithTrans(() =>
            {
                base.Update(item);

                var dbOrderItems = orderItemRepository.GetList(o => o.OrderId == item.Id);

                var modelOrderItems = mapper.Map<List<TOrderItem>>(item.OrderItems);

                var updateOrderItems = modelOrderItems.Intersect(dbOrderItems, new OrderItemComparer());

                var newOrderItems = modelOrderItems.Except(dbOrderItems, new OrderItemComparer());

                var deleteOrderItems = dbOrderItems.Except(updateOrderItems, new OrderItemComparer());

                orderItemRepository.BulkUpdate(updateOrderItems);

                newOrderItems.ToList().ForEach(o => o.OrderId = item.Id);
                orderItemRepository.BulkCreate(newOrderItems);

                orderItemRepository.BulkDelete(deleteOrderItems);
            });
        }

        public override void Delete(Order item)
        {
            unitOfWork.ProcessWithTrans(() =>
            {
                var orderItems = orderItemRepository.GetList(o => o.OrderId == item.Id);

                orderItemRepository.BulkDelete(orderItems);

                base.Delete(item);

            });
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
            return obj.Id.GetHashCode();
        }
    }
}
