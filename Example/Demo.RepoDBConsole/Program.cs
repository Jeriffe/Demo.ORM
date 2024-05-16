using Demo.Data.Models;
using Demo.Data.RepoDBRepository;
using Demo.Infrastructure;
using Demo.Services;
using Microsoft.Data.SqlClient;
using RepoDb;
using System;
using System.Configuration;

namespace Demo.RepoDBConsole
{
    class Program
    {
        static string ConnectionString { get; } = ConfigurationManager.ConnectionStrings["DB"].ConnectionString;

        static void Main(string[] args)
        {


            TestServices();

            TestRepositories();

           

           // RawOperation();

            Console.WriteLine("Welcome to use RepoDB, the fastest ROM in the world!");

            Console.ReadLine();
        }

        private static void TestServices()
        {
            IDbContext context = new SqlDbContext(ConnectionString);
            IUnitOfWork unitOfWork = new UnitOfWork(context);
            var patientRes = new GenericRepository<Patient>(unitOfWork);
            var pservice = new PatientService(unitOfWork, patientRes);
           var plist= pservice.GetAll(null);

            var service = new ReportService(new UnitOfWork(context));


           // var result = service.GetPatientByCareUnitID(3, new PageFilter { PagIndex = 0, PageSize = 100, });
        }

        private static void RawOperation()
        {
            try
            {
                using (var connection = new SqlConnection(ConnectionString))
                {

                    var persons = connection.QueryAll<Patient>();

                    var person = connection.Query<Patient>(p => p.ID == 6);

                    var sql = "SELECT * FROM [dbo].[T_PATIENT] ORDER BY PatientID DESC;";
                    var peoples_ByScript = connection.ExecuteQuery<Patient>(sql);

                    var sqll = "SELECT * FROM [dbo].[T_PATIENT] WHERE Gender=@Gender;";
                    var peoples_ByGender = connection.ExecuteQuery<Patient>(sqll, new { Gender = "F" });


                    var sql1 = "SELECT MAX(PatientID) FROM [dbo].[T_PATIENT];";
                    var maxId = connection.ExecuteScalar<long>(sql1);

                    var sql2 = "SELECT * FROM [dbo].[T_PATIENT] WHERE PatientID=2;";
                    using (var reader = connection.ExecuteReader(sql2))
                    {
                        if (reader.Read())
                        {
                            var p = new Patient
                            {
                                ID = reader.GetInt32(0),
                                MedRecordNumber = reader["MedRecNumber"].ToString()
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ex = ex;
            }
        }

        private static void TestRepositories()
        {
            IDbContext context = new SqlDbContext(ConnectionString);
            IUnitOfWork unitOfWork = new UnitOfWork(context);
            IPatientRepository patientRes = new PatientRepository(unitOfWork);

            var patient = patientRes.GetByKey(2);
            var patient1 = patientRes.Get((Patient p) => p.MedRecordNumber == "938417");

            var patients = patientRes.Get((Patient p) => p.Gender == "F");
            var patientss = patientRes.GetList(new PageFilterWithOrderBy
            {
                PagIndex = 1,
                PageSize = 3,
                OrderBy = "PatientID",
                OrderSorting = OrderSorting.ASC
            });
            int maxId = (int)unitOfWork.ExecuteScalar("SELECT MAX(PatientID) FROM dbo.T_PATIENT");

            //Create
            var np = patientRes.Create(new Patient
            {
                FirstName = $"FirstName{maxId}",
                LastName = $"LastName{maxId}",
                MedRecordNumber = $"MRN{maxId}"
            });

            var newPatient = patientRes.GetByKey(np.ID);
            newPatient.MiddleInitial += "UPDATE";
            //Update
            patientRes.Update(newPatient);

            // Delete
            patientRes.Delete(newPatient);


            unitOfWork.ProcessWithTrans(() =>
            {
                int maxId = (int)unitOfWork.ExecuteScalar("SELECT MAX(PatientID) FROM dbo.T_PATIENT");
                //Create
                var np = patientRes.Create(new Patient
                {
                    FirstName = $"FirstName{maxId}",
                    LastName = $"LastName{maxId}",
                    MedRecordNumber = $"MRN{maxId}"
                });

                var newPatient = patientRes.GetByKey(np.ID);

                newPatient.MiddleInitial += "UPDATE";
                //Update
                patientRes.Update(newPatient);

                //Delete
               patientRes.Delete(newPatient);

               // throw new Exception("Rollback trans");
            });

           
            unitOfWork.ProcessWithTrans(() =>
            {
                int maxId = (int)unitOfWork.ExecuteScalar("SELECT MAX(PatientID) FROM dbo.T_PATIENT");
                patientRes.Delete(new Patient { ID = maxId });
                //Create multiple
                //Update multiple
                //Delete multiple

            });
         

            //query by store procedure
            //var patient_ByCareUnitID_Paging = patientRes.GetPatientByCareUnitID(2, new PageFilter { PagIndex = 1, PageSize = 3 });

            ////query by text
            //var patient_Active_Paging = patientRes.GetActivePatients(new PageFilter { PagIndex = 2, PageSize = 3 });


            ////query by text
            //var patient_AcctNo = patientRes.GetPatientByAccountNumber("AccountNumber938067");

            /*
            var sql2 = "SELECT * FROM [dbo].[T_PATIENT] WHERE PatientID=@PatientID";
            var patient_ById = patientRes.GetSingle(sql2, CommandType.Text, new { PatientID = 2 });


            var sql3 = "SELECT * FROM [dbo].[T_PATIENT] WHERE Gender=@Gender";
            var patient_ByGender = patientRes.GetList(sql3, CommandType.Text, new { GENDER = "F" });


            var sql4 = @"
                        SELECT 
                            * 
                        FROM [dbo].[T_PATIENT] WHERE Gender=@Gender
                        ORDER BY PatientID
                        OFFSET @Offset ROWS
                        FETCH NEXT @PageSize ROWS ONLY";
            var patient_ByGender_Paging = patientRes.GetList(sql4, CommandType.Text, new { GENDER = "F", Offset = 2, PageSize = 2 });
            */
        }

    }
}
