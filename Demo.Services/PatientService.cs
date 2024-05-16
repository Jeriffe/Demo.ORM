using Demo.Data.Models;
using Demo.Infrastructure;
using System.Collections.Generic;

namespace Demo.Services
{

    public interface IPatientService
    {
        IEnumerable<Patient> GetAll(PageFilter pageFilter);
    }


    public class PatientService: IPatientService
    {
        IUnitOfWork unitOfWork;
        IRepository<Patient> patientRepository;
        public PatientService(IUnitOfWork unitOfWork, IRepository<Patient> patientRepository)
        {
            this.unitOfWork = unitOfWork;
            this.patientRepository = patientRepository;
        }

        public IEnumerable<Patient> GetAll(PageFilter pageFilter)
        {
            return patientRepository.GetList();
        }
    }
}
