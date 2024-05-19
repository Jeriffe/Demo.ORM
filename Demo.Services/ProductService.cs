using Demo.Data.Models;
using Demo.DTOs;
using Demo.Infrastructure;
using System.Collections.Generic;

namespace Demo.Services
{

    public class ProductService : BaseAppService<TProduct, Product>
    {
        public ProductService(IUnitOfWork unitOfWork, IRepository<TProduct> repository)
            : base(unitOfWork, repository)
        {
        }
    }
}
