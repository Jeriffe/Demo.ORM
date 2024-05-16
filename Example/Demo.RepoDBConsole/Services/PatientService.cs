using Demo.Data.Models;
using Demo.Data.RepoDBRepository;
using Demo.Infrastructure;
using System.Collections.Generic;

namespace Demo.RepoDBConsole.Services
{
    public class PatientService
    {
        IUnitOfWork unitOfWork;
        IPatientRepository patientRepository;
        public PatientService(IUnitOfWork unitOfWork, IPatientRepository patientRepository)
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
