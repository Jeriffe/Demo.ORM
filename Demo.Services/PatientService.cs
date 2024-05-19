using AutoMapper;
using Demo.Data.Models;
using Demo.DTOs;
using Demo.Infrastructure;
using System.Collections.Generic;

namespace Demo.Services
{

    public class PatientService : BaseAppService<TPatient,Patient>, IAppService<TPatient, Patient>
    {
        public PatientService(IUnitOfWork unitOfWork, IRepository<TPatient> repository,IMapper mapper)
            : base(unitOfWork, repository,mapper)
        {
        }
    }
}
