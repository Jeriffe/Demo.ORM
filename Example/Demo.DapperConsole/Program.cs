using Demo.Data.DapperRepository;
using Demo.Data.DapperRepository.Mappers;
using Demo.Data.Models;
using Demo.Infrastructure;
using System.Configuration;
namespace Demo.DapperConsole
{
    class Program
    {
        static string ConnectionString { get; } = ConfigurationManager.ConnectionStrings["DB"].ConnectionString;

        static void Main(string[] args)
        {
            FluentMappers.Initialize();

            TestRepositories();

            Console.WriteLine("Welcome to use RepoDB, the fastest ROM in the world!");
        }

        private static void TestRepositories()
        {
            IDbContext context = new SqlDbContext(ConnectionString);
            var unitOfWork = new UnitOfWork(context);
            var patientRes = new PatientRepository(unitOfWork);

            var patient = patientRes.Get(o => o.ID == 2);

            var patient1 = patientRes.Get((Patient p) => p.MedRecordNumber == "938417");

            var patients = patientRes.GetList((Patient p) => p.Gender == "F");

            int maxId = (int)unitOfWork.ExecuteScalar("SELECT MAX(PatientID) FROM dbo.T_PATIENT");

            //Create
            var np = patientRes.Create(new Patient
            {
                FirstName = $"FirstName{maxId}",
                LastName = $"LastName{maxId}",
                MedRecordNumber = $"MRN{maxId}",
                BirthDate = DateTime.Now,
                DisChargeDate = DateTime.Now,
            });

            var newPatient = patientRes.Get(o => o.ID == np.ID);
            newPatient.MiddleInitial += "UPDATE";
            //Update
            patientRes.Update(newPatient);

            // Delete
            patientRes.Delete(newPatient);


            unitOfWork.ProcessWithTrans((Action)(() =>
            {
                int maxId = (int)unitOfWork.ExecuteScalar("SELECT MAX(PatientID) FROM dbo.T_PATIENT");
                //Create
                var np = patientRes.Create(new Patient
                {
                    FirstName = $"FirstName{maxId}",
                    LastName = $"LastName{maxId}",
                    MedRecordNumber = $"MRN{maxId}",
                    BirthDate = DateTime.Now,
                    DisChargeDate = DateTime.Now,
                });
                //var np = patientRes.Get(o => o.ID == maxId);

                var newPatient = patientRes.Get(o => o.ID == np.ID);
                newPatient.MiddleInitial += "UPDATE";
                //Update
                patientRes.Update((Patient)newPatient);

                //Delete
                //patientRes.Delete(newPatient);

                // throw new Exception("Rollback trans");
            }));


            unitOfWork.ProcessWithTrans(() =>
            {
                int maxId = (int)unitOfWork.ExecuteScalar("SELECT MAX(PatientID) FROM dbo.T_PATIENT");

                patientRes.Delete(new Patient { ID = maxId });
                //Create multiple
                //Update multiple
                //Delete multiple

            });
          
        }
    }
}