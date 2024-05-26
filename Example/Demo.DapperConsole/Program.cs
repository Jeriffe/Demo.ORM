﻿using AutoMapper;
using Demo.Data.DapperRepository;
using Demo.Data.Models;
using Demo.DBScripts;
using Demo.DTOs;
using Demo.DTOs.Orders;
using Demo.Infrastructure;
using Demo.Services;
using System.Configuration;
using System.Reflection;
namespace Demo.DapperConsole
{
    class Program
    {
        static string ConnectionString { get; } = ConfigurationManager.ConnectionStrings["DB"].ConnectionString;

        static void Main(string[] args)
        {

            RawOperation();

            TestRepositories();

            TestServices();

            Console.WriteLine("Welcome to use RepoDB, the fastest ROM in the world!");

            Console.ReadLine();
        }

        private static void TestServices()
        {
            var context = new SqlDbContext(ConnectionString);
            var unitOfWork = new UnitOfWork(context);
            var patientRepo = new GenericRepository<TPatient>(unitOfWork);

            var _configuration = new MapperConfiguration(config =>
          config.AddMaps(Assembly.GetAssembly(typeof(Patient))));

            var mapper = _configuration.CreateMapper();
            var pservice = new PatientService(unitOfWork, patientRepo, mapper);

            var plist = pservice.GetAll(null);

            var service = new ReportService(new UnitOfWork(context));

            //Use app service
            unitOfWork.ProcessByTrans(() =>
            {
                int maxId = (int)unitOfWork.ExecuteRawScalar("SELECT MAX(PatientID) FROM dbo.T_PATIENT");
                //Create
                var dtoP = new Patient
                {
                    FirstName = $"FirstName{maxId}",
                    LastName = $"LastName{maxId}",
                    MedRecordNumber = $"MRN{maxId}",
                    BirthDate = DateTime.Now,
                    DisChargeDate = DateTime.Now,
                };
                dtoP = pservice.Create(dtoP);
                dtoP.MiddleInitial += "BaseAppSvc";
                pservice.Update(dtoP);
                var updatedP = pservice.GetSingle(dtoP.PatientId);

                // throw new Exception("Rollback trans");
            });

            int maxId = (int)unitOfWork.ExecuteRawScalar("SELECT MAX(PatientID) FROM dbo.T_PATIENT");
            var pppp = pservice.GetSingle(maxId);
            pservice.Delete(pppp);
        }
        private static void TestRepositories()
        {
            var context = new SqlDbContext(ConnectionString);
            var unitOfWork = new UnitOfWork(context);
            var patientRepo = new GenericRepository<TPatient>(unitOfWork);

            //Use repository
            var patient = patientRepo.GetByKey(2);
            var patient1 = patientRepo.Get(p => p.MedRecNumber == "938417");

            var patients = patientRepo.Get(p => p.Gender == "F");

            int maxId = (int)unitOfWork.ExecuteRawScalar("SELECT MAX(PatientID) FROM dbo.T_PATIENT");

            //Create
            var np = patientRepo.Create(new TPatient
            {
                FirstName = $"FirstName{maxId}",
                LastName = $"LastName{maxId}",
                MedRecNumber = $"MRN{maxId}"
            });

            var newPatient = patientRepo.GetByKey(np.PatientId);
            newPatient.MiddleInitial += "UPDATE";
            //Update
            patientRepo.Update(newPatient);

            // Delete
            patientRepo.Delete(newPatient);

            unitOfWork.ProcessByTrans(() =>
            {
                int maxId = (int)unitOfWork.ExecuteRawScalar("SELECT MAX(PatientID) FROM dbo.T_PATIENT");

                var newPatient = patientRepo.GetByKey(maxId);

                newPatient = patientRepo.Create(new TPatient
                {
                    FirstName = $"FirstName{maxId}",
                    LastName = $"LastName{maxId}",
                    MedRecNumber = $"MRN{maxId}"
                });

                newPatient.MiddleInitial += "UPDATE";
                //Update
                patientRepo.Update(newPatient);
            });

            int nmaxId = (int)unitOfWork.ExecuteRawScalar("SELECT MAX(PatientID) FROM dbo.T_PATIENT");
            var nnewPatient = patientRepo.GetByKey(nmaxId);
            // Delete
            patientRepo.Delete(nnewPatient);

        }

        private static void RawOperation()
        {
            try
            {
                var context = new SqlDbContext(ConnectionString);
                var unitOfWork = new UnitOfWork(context);


                var text_sql = ScriptsLoader.Get("ORDER_QUERY_ORDERLITE");
                var orders = unitOfWork.ExecuteRawSql<OrderLite>(text_sql);


                unitOfWork.ExecuteRawScalar("SELECT MAX(PatientID) FROM dbo.T_PATIENT WHERE Gender=@Gender AND PatientId<@PatientId",
                   parameters: [new RawParameter { Name = "@Gender", Value = "F" }, new RawParameter { Name = "@PatientId", Value = 6 }]);

                unitOfWork.ExecuteRawNoQuery("UPDATE dbo.T_PATIENT SET BirthDate=DATEADD(YEAR,-30,GETDATE()) WHERE PatientId=@PatientId",
                    parameters: new RawParameter { Name = "@PatientId", Value = 3 });

                var dataTable = unitOfWork.ExecuteRawSql("SELECT TOP (100) * FROM [ORM_DEMO].[dbo].[T_PATIENT]  WHERE Gender=@Gender AND PatientId<@PatientId",
                        parameters: [new RawParameter { Name = "@Gender", Value = "M" }, new RawParameter { Name = "@PatientId", Value = 3 }]);

                var proc_Sql = ScriptsLoader.Get("PATIENT_QUERY_BY_GENDER");
                var patient = unitOfWork.ExecuteRawSql(proc_Sql, System.Data.CommandType.StoredProcedure,
                    new RawParameter { Name = "@Gender", Value = "M" }
                  , new RawParameter { Name = "@Offset", Value = 1 }
                  , new RawParameter { Name = "@PageSize", Value = 1 });

            }
            catch (Exception ex)
            {
                ex = ex;
            }
        }
    }
}
