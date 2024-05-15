// See https://aka.ms/new-console-template for more information
using Demo.Infrastructure;
using Demo.DapperConsole.Models;
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

            // RawOperation();

            Console.WriteLine("Welcome to use RepoDB, the fastest ROM in the world!");
        }

        private static void TestRepositories()
        {


            IDbContext context = new SqlDbContext(ConnectionString);
            var unitOfWork = new UnitOfWork(context);
            var patientRes = new PatientRepository(unitOfWork);

            var patient = patientRes.Get(o => o.ID == 2);

            int maxId = (int)unitOfWork.ExecuteScalar("SELECT MAX(PatientID) FROM dbo.T_PATIENT");
            var nps = patientRes.Create(new Patient
            {
                FirstName = $"FirstName{maxId}",
                LastName = $"LastName{maxId}",
                MedRecordNumber = $"MRN{maxId}",
                BirthDate = DateTime.Now,
                DisChargeDate = DateTime.Now,
            });




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


            unitOfWork.ProcessWithTrans(() =>
            {
                int maxId = (int)unitOfWork.ExecuteScalar("SELECT MAX(PatientID) FROM dbo.T_PATIENT");

                patientRes.Delete(new Patient { ID = maxId });
                //Create multiple
                //Update multiple
                //Delete multiple

            });



            var patient1 = patientRes.Get((Patient p) => p.MedRecordNumber == "938417");

            var patients = patientRes.GetList((Patient p) => p.Gender == "F");
        }

    }
}