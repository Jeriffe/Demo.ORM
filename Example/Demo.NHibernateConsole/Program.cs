using Demo.Data.Models;
using Demo.Data.NHibernateRepository;
using NHibernate;
using System;
using System.Configuration;
using AutoMapper;
using Demo.Services;
using Demo.DTOs;
using System.Reflection;
using Demo.Infrastructure;

namespace Demo.NHibernateConsole
{
    class Program
    {
        static string ConnectionString { get; } = ConfigurationManager.ConnectionStrings["DB"].ConnectionString;

        static void Main(string[] args)
        {

          //  TestRepositories();

            TestServices();



            // RawOperation();

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
                int maxxId = (int)unitOfWork.ExecuteScalar("SELECT MAX(PatientID) FROM dbo.T_PATIENT");
                //Create
                var dtoP = new Patient
                {
                    FirstName = $"FirstName{maxxId}",
                    LastName = $"LastName{maxxId}",
                    MedRecordNumber = $"MRN{maxxId}",
                    BirthDate = DateTime.Now,
                    DisChargeDate = DateTime.Now,
                };
                dtoP = pservice.Create(dtoP);

                //https://www.cnblogs.com/Y-S-X/p/8347152.html
                dtoP.MiddleInitial += "BaseAppSvc";
                pservice.Update(dtoP);


                // throw new Exception("Rollback trans");
            });

            int maxId = (int)unitOfWork.ExecuteScalar("SELECT MAX(PatientID) FROM dbo.T_PATIENT");
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

            int maxId = (int)unitOfWork.ExecuteScalar("SELECT MAX(PatientID) FROM dbo.T_PATIENT");

            //Create
            var np = patientRepo.Create(new TPatient
            {
                FirstName = $"FirstName{maxId}",
                LastName = $"LastName{maxId}",
                MedRecNumber = $"MRN{maxId}"
            });

            np.MiddleInitial += "UPDATE";
            //Update
            patientRepo.Update(np);

            // Delete
            patientRepo.Delete(np);

            unitOfWork.ProcessByTrans(() =>
            {
                int maxxId = (int)unitOfWork.ExecuteScalar("SELECT MAX(PatientID) FROM dbo.T_PATIENT");

                var nnewPatient = patientRepo.Create(new TPatient
                {
                    FirstName = $"FirstName{maxxId}",
                    LastName = $"LastName{maxxId}",
                    MedRecNumber = $"MRN{maxxId}"
                });

                nnewPatient.MiddleInitial += "UPDATE";
                //Update
                patientRepo.Update(nnewPatient);
            });

            int nmaxId = (int)unitOfWork.ExecuteScalar("SELECT MAX(PatientID) FROM dbo.T_PATIENT");
            var nnnewPatient = patientRepo.GetByKey(nmaxId);
            // Delete
            patientRepo.Delete(nnnewPatient);

        }

    }
}

 