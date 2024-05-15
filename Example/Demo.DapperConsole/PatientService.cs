using Demo.Infrastructure;

namespace Demo.SqlSugarConsole.Services
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
