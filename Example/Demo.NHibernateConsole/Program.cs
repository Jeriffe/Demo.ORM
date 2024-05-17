using Demo.Data.Models;
using Demo.Data.NHibernateRepository;
using Demo.Infrastructure;
using Demo.Services;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using System;
using System.Configuration;
using System.Linq;
using System.Transactions;

namespace Demo.NHibernateConsole
{
    class Program
    {
        static string ConnectionString { get; } = ConfigurationManager.ConnectionStrings["DB"].ConnectionString;

        static void Main(string[] args)
        {
            //var p1 =  session.Get<Patient>(2);
            //var rs = session.Query<Patient>().ToList();
            //var results = session.QueryOver<Patient>().Where(x => x.Gender == "F").List();

            var context = new SqlDbContext(ConnectionString);
            var unitOfWork = new UnitOfWork(context);
            var patientRes = new GenericRepository<Patient>(unitOfWork);

            var mID = patientRes.GetByKey(3);
            var Pp = new Patient
            {
                FirstName = $"FirstName{mID.ID}",
                LastName = $"LastName{mID.ID}",
                MedRecordNumber = $"MRN{mID.ID}"
            };
            var patient = patientRes.Create(Pp);

            unitOfWork.ProcessByTrans(
                () =>
                {
                   var p1 = patientRes.Get(o=>o.ID==2);
                    if (p1 != null)
                    {
                        var maxId = p1.ID;

                        var p = new Patient
                        {
                            FirstName = $"FirstName{maxId}",
                            LastName = $"LastName{maxId}",
                            MedRecordNumber = $"MRN{maxId}"
                        };

                        patientRes.Create(p);

                        p.MiddleInitial = "MUpdate";
                        patientRes.Update(p);

                    }
                }
            );

            Console.WriteLine("Welcome to use RepoDB, the fastest ROM in the world!");

            TestServices();
            //Test();
        }

        private static void Test()
        {
            //var sessionFactory = CreateSessionFactory();
            //using (var session = sessionFactory.OpenSession())
            //{
            //    var p1 = session.Get<Patient>(2);
            //    var rs = session.Query<Patient>().ToList();
            //    var results = session.QueryOver<Patient>().Where(x => x.Gender == "F").List();

            //    var unitOfWork = new UnitOfWork(session);
            //    var patientRes = new GenericRepository<Patient>(unitOfWork);

            //    var mID = p1.ID;
            //    var Pp = new Patient
            //    {
            //        FirstName = $"FirstName{mID}",
            //        LastName = $"LastName{mID}",
            //        MedRecordNumber = $"MRN{mID}"
            //    };
            //    var patient = patientRes.Create(Pp);

            //    unitOfWork.ProcessByTrans(
            //        () =>
            //        {
            //            if (p1 != null)
            //            {
            //                var maxId = p1.ID;

            //                var p = new Patient
            //                {
            //                    FirstName = $"FirstName{maxId}",
            //                    LastName = $"LastName{maxId}",
            //                    MedRecordNumber = $"MRN{maxId}"
            //                };

            //                patientRes.Create(p);

            //                p.MiddleInitial = "MUpdate";
            //                patientRes.Update(p);

            //            }
            //        }
            //    );

            //    // retrieve all stores and display them
            //    using (var transaction = new TransactionScope())
            //    {
            //        if (p1 != null)
            //        {
            //            var maxId = p1.ID;

            //            var p = new Patient
            //            {
            //                FirstName = $"FirstName{maxId}",
            //                LastName = $"LastName{maxId}",
            //                MedRecordNumber = $"MRN{maxId}"
            //            };

            //            patientRes.Create(p);

            //            p.MiddleInitial = "MUpdate";
            //            patientRes.Update(p);

            //            transaction.Complete();

            //        }
            //    }

            //}
            //Console.WriteLine("Welcome to use RepoDB, the fastest ROM in the world!");
        }

        private static void TestServices()
        {
            //var sessionFactory = CreateSessionFactory();
            //using (var session = sessionFactory.OpenSession())
            //{
            //    IUnitOfWork unitOfWork = new UnitOfWork(session);
            //    var patientRes = new GenericRepository<Patient>(unitOfWork);
            //    var pservice = new PatientService(unitOfWork, patientRes);
            //    var plist = pservice.GetAll(null);

            //    var service = new ReportService(unitOfWork);


            //    // var result = service.GetPatientByCareUnitID(3, new PageFilter { PagIndex = 0, PageSize = 100, });
            //}
        }

        static ISessionFactory CreateSessionFactory()
        {
            return Fluently.Configure()
                .Database(MsSqlConfiguration.MsSql2012
                    .ConnectionString(ConnectionString))
                .Mappings(m =>
                    m.FluentMappings.AddFromAssemblyOf<Program>())
                .ExposeConfiguration(BuildSchema)
                .BuildSessionFactory();
        }

        static void BuildSchema(NHibernate.Cfg.Configuration config)
        {
            // this NHibernate tool takes a configuration (with mapping info in)
            // and exports a database schema from it
            new SchemaExport(config).Create(false, false);
        }

    }
}
