using AutoMapper;
using Demo.Data.Models;
using Demo.Date.EFCoreRepository;
using Demo.DBScripts;
using Demo.DTOs;
using Demo.DTOs.Orders;
using Demo.Infrastructure;
using Demo.Services;
using Microsoft.Extensions.Logging;
using System.Configuration;
using System.Data;
using System.Reflection;


namespace Demo.EFCoreConsole
{
    internal class Program
    {
        static string ConnectionString { get; set; } = ConfigurationManager.ConnectionStrings["DB"].ConnectionString;
        static DataProviderType ProviderName { get; set; } = DataProviderType.SQLServer;
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

            log.LogInformation("Welcome to use Entity Framework Core, the best ORM in the .NET Platform!");


            try
            {
                //ProviderName = DataProviderType.Sqlite;
                //ConfigureConnectionString();

                // //SqlServer
                ProviderName = DataProviderType.SQLServer;

                RawOperation();

                //TestPatientRepository();

                //TestProductRepository();

                //TestOrders();

                //TestServices();
            }
            catch (Exception ex)
            {
                ex = ex;
            }


        }

        private static void ConfigureConnectionString()
        {
            switch (ProviderName)
            {
                case DataProviderType.SQLServer:
                    break;
                case DataProviderType.PostgreSQL:
                    break;
                case DataProviderType.MySQL:
                    break;
                case DataProviderType.Sqlite:
                    var baseDir = AppDomain.CurrentDomain.BaseDirectory;
                    var dbFullName = Path.Combine(baseDir, @"Data\ORM_DEMO.db");

                    ConnectionString = $"Data Source = {dbFullName};";
                    break;
                default:
                    break;
            }
        }

        private static EFCoreDBcontext BuildSqlContext()
        {
            switch (ProviderName)
            {
                case DataProviderType.PostgreSQL:
                case DataProviderType.MySQL:
                    throw new NotImplementedException("DataProviderType");
                case DataProviderType.Sqlite:
                    return new EFCoreDBcontext(ConnectionString, DataProviderType.Sqlite);
                case DataProviderType.SQLServer:
                default:
                    //SqlServer
                    ProviderName = DataProviderType.SQLServer;
                    // return new EFCoreDBcontext(ConnectionString, ProviderName);
                    return new EFCoreDBcontext(ConnectionString);
            }
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
            var pOrder = svc.GetSingle(maxId);

            var plist = svc.GetAll(null);



            //Create
            var dtoP = new Order
            {
                Customer = new Customer { Id = 4 },
                Description = $"{maxId}",
                TotalPrice = 8888.88m,
                OrderItems = new List<OrderItem>
                {
                    new OrderItem
                    {
                        ProductId=2,Price=99.99m,
                        Description="Desc,P2,Price99.99",
                        CreateDate=DateTime.Now
                    },
                    new OrderItem
                    {
                        ProductId=10,Price=88.88m,
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
            order.TotalPrice = 999.99m;
            order.OrderItems.RemoveAt(0);
            order.OrderItems[0].Price += 10;
            order.OrderItems.Add(new OrderItem
            {
                OrderId = order.Id,
                ProductId = 5,
                Price = 66.66m,
                Description = "Desc,P5,Price66.66",
                CreateDate = DateTime.Now
            });

            svc.Update(order);

            //Delete

            var orderAfterUpdate = svc.GetSingle(maxId);
            svc.Delete(orderAfterUpdate);

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

            var logP = loggerFactory.CreateLogger<PatientService>();
            var pservice = new PatientService(unitOfWork, patientRepo, mapper, logP);

            var plist = pservice.GetAll(null);

            var logR = loggerFactory.CreateLogger<ReportService>();
            var service = new ReportService(new UnitOfWork(context), logR);

            //Use app service
            unitOfWork.ProcessByTrans(() =>
            {
                var maxId = unitOfWork.ExecuteRawScalar("SELECT MAX(PatientID) FROM T_PATIENT");
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

            var maxId = unitOfWork.ExecuteRawScalar("SELECT MAX(PatientID) FROM T_PATIENT");
            var pppp = pservice.GetSingle(maxId);
            pservice.Delete(pppp);
        }
        private static void TestProductRepository()
        {
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

        private static void TestPatientRepository()
        {
            var context = BuildSqlContext();
            var unitOfWork = new UnitOfWork(context);
            var repo = new GenericRepository<TPatient>(unitOfWork);

            var sql = "SELECT * FROM T_PATIENT WHERE PatientID = @PatientId";
            var list2 = unitOfWork.ExecuteRawSql<TPatient>(sql: sql, parameters: new RawParameter { Name = "@PatientId", Value = 1 });

            var sqlUpdate = "update T_PATIENT set FirstName = @FirstName where PatientID = @PatientId";
            unitOfWork.ExecuteRawNoQuery(sqlUpdate, CommandType.Text,
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

            var patient1 = repo.Get(o => o.PatientId == 1);
            patient1.FirstName = "change";
            repo.Update(patient1);

            var list = repo.GetList();
            var list1 = repo.GetList(o => o.PatientId == 2);
        }

        private static void RawOperation()
        {
            switch (ProviderName)
            {
                case DataProviderType.SQLServer:
                    SqlServerRawOperation();
                    break;
                case DataProviderType.PostgreSQL:
                    break;
                case DataProviderType.MySQL:
                    break;
                case DataProviderType.Sqlite:
                    SqliteRawOperation();
                    break;
                default:
                    break;
            }
        }
        private static void SqliteRawOperation()
        {
            try
            {
                var context = BuildSqlContext();
                var unitOfWork = new UnitOfWork(context);

                var text_sql = ScriptsLoader.Get("ORDER_QUERY_ORDERLITE");
                var orders = unitOfWork.ExecuteRawSql<OrderLite>(text_sql);


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

        private static void SqlServerRawOperation()
        {
            try
            {
                var context = new EFCoreDBcontext(ConnectionString);
                var unitOfWork = new UnitOfWork(context);

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
    }
}
