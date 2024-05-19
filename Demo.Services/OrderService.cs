using Demo.Data.Models;
using Demo.DTOs;
using Demo.Infrastructure;
using System.Collections.Generic;

namespace Demo.Services
{

    public class OrderService : BaseAppService<TOrder, Order>, IAppService<TOrder, Order>
    {
        public OrderService(IUnitOfWork unitOfWork, IRepository<TOrder> repository)
            : base(unitOfWork, repository)
        {
        }
    }
}
