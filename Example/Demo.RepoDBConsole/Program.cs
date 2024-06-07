﻿using AutoMapper;
using Demo.Data.Models;
using Demo.Data.RepoDBRepository;
using Demo.DBScripts;
using Demo.DTOs;
using Demo.DTOs.Orders;
using Demo.Infrastructure;
using Demo.RawSql;
using Demo.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Reflection;

namespace Demo.RepoDBConsole
{
    class Program
    {
        static string ConnectionString { get; set; } = ConfigurationManager.ConnectionStrings["DB"].ConnectionString;
        static ILoggerFactory loggerFactory;
        static void Main(string[] args)
        {

            //NuGet\Install-Package Microsoft.Extensions.Logging -Version 8.0.0
            loggerFactory = LoggerFactory.Create(builder =>
                  //NuGet\Install-Package Microsoft.Extensions.Logging.Console -Version 8.0.0
                  builder.AddSimpleConsole(options =>
                  {
                      options.IncludeScopes = true;
                      options.SingleLine = true;
                      options.TimestampFormat = "yyyy-MM-dd HH:mm:ss ";
                  }));

            var log = loggerFactory.CreateLogger<Program>();

            ConfigureSqlite();

            TestSqliteRepositories();


         //   TestRepositories();


            TestOrders();

            //RawOperation();

            TestServices();

            log.LogInformation("Welcome to use RepoDB, the fastest ORM in the world!");

            Console.ReadLine();
        }
        private static RepoDbContext BuildSqlContext()
        {
            //Sqlite
            FluentMappers.Sql_Schema = string.Empty;

            return new RepoDbContext(ConnectionString, DataProviderType.Sqlite);

            //SqlServer
            // return new SqlDbContext(ConnectionString, DataProviderType.SQLServer);
            // return BuildSqlContext();
        }
        private static void TestSqliteRepositories()
        {
          

            SqliteRawOperation();

            var context = BuildSqlContext();
            var unitOfWork = new UnitOfWork(context);
            var repo = new GenericRepository<TProduct>(unitOfWork);

            //Use repository
            var item = repo.GetByKey(2);
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

        private static void ConfigureSqlite()
        {
            var baseDir = AppDomain.CurrentDomain.BaseDirectory;
            var dbFullName = Path.Combine(baseDir, @"Data\ORM_DEMO.db");

            ConnectionString = $"Data Source = {dbFullName};";
        }

        private static void TestOrders()
        {
            var context = BuildSqlContext();

            var unitOfWork = new UnitOfWork(context);

            var orderRepo = new GenericRepository<TOrder>(unitOfWork);
            var orderItemRep = new GenericRepository<TOrderItem>(unitOfWork);

            var _configuration = new MapperConfiguration(config =>
          config.AddMaps(Assembly.GetAssembly(typeof(Patient))));

            var mapper = _configuration.CreateMapper();

            var log = loggerFactory.CreateLogger<OrderSvc>();
            var svc = new OrderSvc(unitOfWork, orderRepo, orderItemRep, mapper, log);




            var maxId = (long)unitOfWork.ExecuteRawScalar("SELECT MAX([Id]) FROM T_Order");

            var maxOrder = svc.GetSingle(maxId);


            var plist = svc.GetAll(null);

            //Create
            var dtoP = new Order
            {
                Customer = new Customer { Id = 4 },
                Description = $"{maxId}",
                TotalPrice = 8888.88d,
                OrderItems = new List<OrderItem>
                {
                    new OrderItem
                    {
                        ProductId=2,Price=99.99d,
                        Description="Desc,P2,Price99.99",
                        CreateDate=DateTime.Now
                    },
                    new OrderItem
                    {
                        ProductId=10,Price=88.88d,
                        Description="Desc,P10,Price88.88",
                        CreateDate=DateTime.Now
                    }
                }
            };
            dtoP = svc.Create(dtoP);


            //Update
            maxId = (long)unitOfWork.ExecuteRawScalar("SELECT MAX([Id]) FROM T_Order");
            var order = svc.GetSingle(maxId);
            var orderItems = orderItemRep.GetList(o => o.OrderId == order.Id);
            order.OrderItems = mapper.Map<List<OrderItem>>(orderItems);
            order.TotalPrice = 999.99;
            order.OrderItems.RemoveAt(0);
            order.OrderItems[0].Price += 10;
            order.OrderItems.Add(new OrderItem
            {
                ProductId = 5,
                Price = 66.66,
                Description = "Desc,P5,Price66.66",
                CreateDate = DateTime.Now
            });

            svc.Update(order);

            //Delete
            svc.Delete(order);

            maxId = (long)unitOfWork.ExecuteRawScalar("SELECT MAX(Id) FROM T_Order");
        }

        private static void TestServices()
        {
            var context = BuildSqlContext();

            var unitOfWork = new UnitOfWork(context);

            var patientRepo = new GenericRepository<TPatient>(unitOfWork);

            var _configuration = new MapperConfiguration(config =>
          config.AddMaps(Assembly.GetAssembly(typeof(Patient))));

            var mapper = _configuration.CreateMapper();

            var log = loggerFactory.CreateLogger<PatientService>();
            var pservice = new PatientService(unitOfWork, patientRepo, mapper, log);

            var plist = pservice.GetAll(null);

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
                var context = BuildSqlContext();
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
            var context = BuildSqlContext();
            var unitOfWork = new UnitOfWork(context);
            var patientRepo = new PatientRepository(unitOfWork);

            //Use repository
            var patient = patientRepo.GetByKey(2);
            var patient1 = patientRepo.Get(p => p.MedRecNumber == "938417");

            var patients = patientRepo.Get(p => p.Gender == "F");
            var maxId = unitOfWork.ExecuteRawScalar("SELECT MAX(PatientID) FROM T_PATIENT");

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
                var maxId = unitOfWork.ExecuteRawScalar("SELECT MAX(PatientID) FROM T_PATIENT");

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

            var nmaxId = unitOfWork.ExecuteRawScalar("SELECT MAX(PatientID) FROM T_PATIENT");
            var nnewPatient = patientRepo.GetByKey(nmaxId);
            // Delete
            patientRepo.Delete(nnewPatient);

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
                        parameters: [new RawParameter { Name = "@Gender", Value = "M" }, new RawParameter { Name = "@PatientId", Value = 3 }]);

            }
            catch (Exception ex)
            {
                ex = ex;
            }
        }

    }
}
