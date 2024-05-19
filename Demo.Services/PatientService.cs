using Demo.Data.Models;
using Demo.Infrastructure;
using System.Collections.Generic;

namespace Demo.Services
{

    public interface IPatientService
    {
        IEnumerable<Patient> GetAll(PageFilter pageFilter);
    }


    public class PatientService : BaseAppService<Patient>, IPatientService
    {
        public PatientService(IUnitOfWork unitOfWork, IRepository<Patient> patientRepository)
            : base(unitOfWork, patientRepository)
        {
        }

        public IEnumerable<Patient> GetAll(PageFilter pageFilter)
        {
            return entityRepository.GetList();
        }
    }
}
