using Demo.Data.Models;
using Demo.Date.EFCoreRepository;
using Demo.Infrastructure;
using Microsoft.Data.SqlClient;
using System.Configuration;
using System.Data;


namespace Demo.EFCoreConsole
{
    internal class Program
    {
        static string ConnectionString { get; set; } = ConfigurationManager.ConnectionStrings["DB"].ConnectionString;

        static void Main(string[] args)
        {
            TestSqliteRepositories();

            TestDBContext();

            TestRepository();
        }
        private static void ConfigureSqlite()
        {
            var baseDir = AppDomain.CurrentDomain.BaseDirectory;
            var dbFullName = Path.Combine(baseDir, @"Data\ORM_DEMO.db");

            ConnectionString = $"Data Source = {dbFullName};";
        }

        private static EFCoreDBcontext BuildSqlContext()
        {
            ConfigureSqlite();

            return new EFCoreDBcontext(ConnectionString, DataProviderType.Sqlite);

            //SqlServer
            // return new SqlDbContext(ConnectionString, DataProviderType.SQLServer);
            // return BuildSqlContext();
        }

        private static void TestSqliteRepositories()
        {
          var  context1 = BuildSqlContext();

            context1 = null;

            var  context2 = BuildSqlContext();
            context1 = null;

            var context3 = BuildSqlContext();

            context3 = null;

         //   SqliteRawOperation();

            var context = BuildSqlContext();
            var unitOfWork = new UnitOfWork(context);
            var repo = new GenericRepository<TProduct>(unitOfWork);

            //Use repository
            var item = repo.GetByKey(2l);
            var items1 = repo.Get(p => p.Name == "Laptop Pro");

            var items = repo.GetList(p => p.Price > 100m);

            var maxId = unitOfWork.ExecuteRawScalar("SELECT MAX(Id) FROM T_Product");

            //Create
            var np = repo.Create(new TProduct
            {
                Name = $"tName{maxId}",
                Description = $"Description{maxId}",
                Price = 998.99m
            });

            var newPatient = repo.GetByKey(np.Id);
            newPatient.Description += "UPDATE";
            //Update
            repo.Update(newPatient);

            // Delete
            repo.Delete(newPatient);

            unitOfWork.ProcessByTrans(() =>
            {
                var maxId = unitOfWork.ExecuteRawScalar("SELECT MAX(Id) FROM T_Product");

                var sss = repo.GetByKey(maxId);

                sss = repo.Create(new TProduct
                {
                    Name = $"tName{maxId}",
                    Description = $"Description{maxId}",
                    Price = 998.99m
                });

                sss.Description += "UPDATE";

                //Update
                repo.Update(sss);
            });

            var nmaxId = unitOfWork.ExecuteRawScalar("SELECT MAX(Id) FROM T_Product");
            var lastproduct = repo.GetByKey(nmaxId);
            // Delete
            repo.Delete(lastproduct);

        }
        private static void SqliteRawOperation()
        {
            try
            {
                var context = BuildSqlContext();
                var unitOfWork = new UnitOfWork(context);


                var id = unitOfWork.ExecuteRawScalar("SELECT MAX(PatientID) FROM T_PATIENT WHERE Gender=@Gender AND PatientId<@PatientId",
                      parameters: [new RawParameter { Name = "@Gender", Value = "F" }, new RawParameter { Name = "@PatientId", Value = 6 }]);

                //SELECT DATE('2050-08-21', '+10 days'); DATE('2050-08-21', '+1 month'); DATE('now', '+1 years');
                unitOfWork.ExecuteRawNoQuery("UPDATE T_PATIENT SET BirthDate=DATE('now', '-1 month') WHERE PatientId=@PatientId",
                    parameters: new RawParameter { Name = "@PatientId", Value = 1 });

                //SELECT  column_list  FROM table LIMIT {ROW_COUNT};
                var dataTable = unitOfWork.ExecuteRawSql("SELECT  * FROM  [T_PATIENT]  WHERE Gender=@Gender AND PatientId<@PatientId LIMIT 100",
                        parameters: [new RawParameter { Name = "@Gender", Value = "M" }, new RawParameter { Name = "@PatientId", Value = 1 }]);

            }
            catch (Exception ex)
            {
                ex = ex;
            }
        }

        private static void TestRepository()
        {
            var context = BuildSqlContext();
            var unitOfWork = new UnitOfWork(context);
            var repo = new GenericRepository<TPatient>(unitOfWork);

            var sql = "select * from T_PATIENT where PatientID = @PatientId";
            var list2 = unitOfWork.ExecuteRawSql<TPatient>(sql: sql, parameters: new RawParameter { Name = "@PatientId", Value = 1 });

            var sqlUpdate = "update T_PATIENT set FirstName = @FirstName where PatientID = @PatientId";
            unitOfWork.ExecuteRawSql<TPatient>(sqlUpdate, CommandType.Text,
                parameters: [new RawParameter { Name = "@FirstName", Value = "hahaha" }, new RawParameter { Name = "@PatientId", Value = 1 }]);

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
            using (var ctx = BuildSqlContext())
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
