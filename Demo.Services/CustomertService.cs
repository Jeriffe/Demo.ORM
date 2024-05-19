using Demo.Data.Models;
using Demo.DTOs;
using Demo.Infrastructure;
using System.Collections.Generic;

namespace Demo.Services
{

    public class CustomerService : BaseAppService<TCustomer, Customer>
    {

        public CustomerService(IUnitOfWork unitOfWork, IRepository<TCustomer> patientRepository)
            : base(unitOfWork, patientRepository)
        {
        }
    }
}
