using Demo.Data.Models;
using Demo.Date.EFCoreRepository;
using Microsoft.Data.SqlClient;
using System.Configuration;
using System.Data;


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

            var sql = "select * from T_PATIENT where PatientID = @PatientId";
            var list2 = unitOfWork.ExecuteRawSql<TPatient>(sql: sql, parameters: new SqlParameter("@PatientId", 1));

            var sqlUpdate = "update T_PATIENT set FirstName = @FirstName where PatientID = @PatientId";
            unitOfWork.ExecuteRawNoQuery<TPatient>(sqlUpdate, CommandType.Text, new SqlParameter("@FirstName", "hahaha"), new SqlParameter("@PatientId", 1));

            var patient = repo.GetByKey(2);

            var patientNew = new TPatient()
            {
                FirstName = "F8",
                LastName = "LastName938419",
                BirthDate = DateTime.Now,
                Gender = "M",
                MedRecNumber = "938419",
                SiteId = 999
            };
            var patientInserted = repo.Create(patientNew);
            repo.Delete(patientInserted);

            var patient1 = repo.Get(o => o.PatientId == 6);
            patient1.FirstName = "change";
            repo.Update(patient1);

            var list = repo.GetList();
            var list1 = repo.GetList(o=>o.PatientId == 2);



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
