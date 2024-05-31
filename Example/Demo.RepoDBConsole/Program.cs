using AutoMapper;
using Demo.Data.Models;
using Demo.Data.RepoDBRepository;
using Demo.DBScripts;
using Demo.DTOs;
using Demo.DTOs.Orders;
using Demo.Infrastructure;
using Demo.RawSql;
using Demo.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Reflection;

namespace Demo.RepoDBConsole
{
    class Program
    {
        static string ConnectionString { get; } = ConfigurationManager.ConnectionStrings["DB"].ConnectionString;

        static void Main(string[] args)
        {

            TestOrders();

            //RawOperation();

            //TestRepositories();

            //TestServices();

            Console.WriteLine("Welcome to use RepoDB, the fastest ROM in the world!");

            Console.ReadLine();
        }
        private static void TestOrders()
        {
            var context = new SqlDbContext(ConnectionString);

            var unitOfWork = new UnitOfWork(context);

            var orderRepo = new GenericRepository<TOrder>(unitOfWork);
            var orderItemRep = new GenericRepository<TOrderItem>(unitOfWork);

            var _configuration = new MapperConfiguration(config =>
          config.AddMaps(Assembly.GetAssembly(typeof(Patient))));

            var mapper = _configuration.CreateMapper();
            var svc = new OrderSvc(unitOfWork, orderRepo, orderItemRep, mapper);

            var plist = svc.GetAll(null);


            var maxId = (long)unitOfWork.ExecuteRawScalar("SELECT MAX([Id]) FROM dbo.T_Order");
            //Create
            var dtoP = new Order
            {
                Customer = new Customer { Id = 4 },
                Description = $"{maxId}",
                TotalPrice = 8888.88,
                OrderItems = new List<OrderItem>
                {
                    new OrderItem
                    {
                        ProductId=2,Price=99.99,
                        Description="Desc,P2,Price99.99",
                        CreateDate=DateTime.Now
                    },
                    new OrderItem
                    {
                        ProductId=10,Price=88.88,
                        Description="Desc,P10,Price88.88",
                        CreateDate=DateTime.Now
                    }
                }
            };
            dtoP = svc.Create(dtoP);


            //Update
            maxId = (long)unitOfWork.ExecuteRawScalar("SELECT MAX([Id]) FROM dbo.T_Order");
            var order = svc.GetSingle(maxId);
            var orderItems = orderItemRep.GetList(o => o.OrderId == order.Id);
            order.OrderItems = mapper.Map<List<OrderItem>>(orderItems);
            order.TotalPrice = 999.99;
            order.OrderItems.RemoveAt(0);
            order.OrderItems[0].Price += 10;
            order.OrderItems.Add(new OrderItem {
                ProductId = 5,
                Price = 66.66,
                Description = "Desc,P5,Price66.66",
                CreateDate = DateTime.Now
            });

            svc.Update(order);

            //Delete
            svc.Delete(order);

            maxId = (long)unitOfWork.ExecuteRawScalar("SELECT MAX(Id) FROM dbo.T_Order");
        }

        private static void TestServices()
        {
            var context = new SqlDbContext(ConnectionString);

            var unitOfWork = new UnitOfWork(context);

            var patientRepo = new GenericRepository<TPatient>(unitOfWork);

            var _configuration = new MapperConfiguration(config =>
          config.AddMaps(Assembly.GetAssembly(typeof(Patient))));

            var mapper = _configuration.CreateMapper();
            var pservice = new PatientService(unitOfWork, patientRepo, mapper, null);

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

            // var result = service.GetPatientByCareUnitID(3, new PageFilter { PagIndex = 0, PageSize = 100, });
        }

        private static void RawOperation()
        {
            try
            {
                var context = new SqlDbContext(ConnectionString);
                var sqlExecutor = new DapperRawExecutor();
                var unitOfWork = new UnitOfWork(context, sqlExecutor);

                //var unitOfWork = new UnitOfWork(context);


                var text_sql = ScriptsLoader.Get("ORDER_QUERY_ORDERLITE");
                var orders = unitOfWork.ExecuteRawSql<OrderLite>(text_sql);



                unitOfWork.ExecuteRawScalar("SELECT MAX(PatientID) FROM dbo.T_PATIENT WHERE Gender=@Gender AND PatientId<@PatientId",
               parameters: new List<RawParameter> {
                    new RawParameter { Name = "@Gender", Value = "F" },
                    new RawParameter { Name = "@PatientId", Value = 6 }}.ToArray());

                unitOfWork.ExecuteRawNoQuery("UPDATE dbo.T_PATIENT SET BirthDate=DATEADD(YEAR,-30,GETDATE()) WHERE PatientId=@PatientId",
                    parameters: new RawParameter { Name = "@PatientId", Value = 3 });

                var dataTable = unitOfWork.ExecuteRawSql("SELECT TOP (100) * FROM [ORM_DEMO].[dbo].[T_PATIENT]  WHERE Gender=@Gender AND PatientId<@PatientId",
                        parameters: new List<RawParameter> { new RawParameter { Name = "@Gender", Value = "M" }, new RawParameter { Name = "@PatientId", Value = 3 } }.ToArray());

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

        private static void TestRepositories()
        {
            var context = new SqlDbContext(ConnectionString);
            var unitOfWork = new UnitOfWork(context);
            var patientRepo = new PatientRepository(unitOfWork);

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

    }
}
