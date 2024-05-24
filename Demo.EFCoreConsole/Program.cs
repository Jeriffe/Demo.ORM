using Demo.Data.Models;
using Demo.Date.EFCoreRepository;
using System.Configuration;


namespace Demo.EFCoreConsole
{
    internal class Program
    {
        static string ConnectionString { get; } = ConfigurationManager.ConnectionStrings["DB"].ConnectionString;

        static void Main(string[] args)
        {
            TestDBContext();

            TestRepository();
        }

        private static void TestRepository()
        {
            var context = new SqlDBcontext(ConnectionString);
            var unitOfWork = new UnitOfWork(context);
            var repo = new GenericRepository<TPatient>(unitOfWork);
            var patient = repo.GetByKey(2);
        }

        private static void TestDBContext()
        {
            using (var ctx = new SqlDBcontext(ConnectionString))
            {
                var patients = ctx.Patients;
                foreach (var patient in patients)
                {
                    Console.WriteLine(patient.LastName + patient.FirstName);
                }
            }
        }
    }
}
