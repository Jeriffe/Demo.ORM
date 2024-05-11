using Demo.Infrastructure;
using Demo.RepoDBConsole.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
