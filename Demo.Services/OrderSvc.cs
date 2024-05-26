using AutoMapper;
using Demo.Data.Models;
using Demo.DTOs;
using Demo.Infrastructure;

namespace Demo.Services
{
    public interface IOrderSvc : IAppService<TOrder, Order>
    {
    }
    public class OrderSvc : BaseAppService<TOrder, Order>, IOrderSvc
    {
        public OrderSvc(IUnitOfWork unitOfWork, IRepository<TOrder> repository, IMapper mapper)
            : base(unitOfWork, repository, mapper)
        {


        }
    }

}
