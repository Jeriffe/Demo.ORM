using Demo.Data.NHibernateRepository;
using Demo.Models;
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
            var sessionFactory = CreateSessionFactory();
            using (var session = sessionFactory.OpenSession())
            {
                var p1 = session.Get<Patient>(2);
                var rs = session.Query<Patient>().ToList();
                var results = session.QueryOver<Patient>().Where(x => x.Gender == "F").List();




                var unitOfWork = new UnitOfWork(session);
                var patientRes = new GenericRepository<Patient>(unitOfWork);

                var mID = p1.ID;
                var Pp = new Patient
                {
                    FirstName = $"FirstName{mID}",
                    LastName = $"LastName{mID}",
                    MedRecordNumber = $"MRN{mID}"
                };
                var patient = patientRes.Create(Pp);

                unitOfWork.ProcessByTrans(
                    () =>
                        {
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

                // retreive all stores and display them
                using (var transaction = new TransactionScope())
                {
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

                        transaction.Complete();

                    }
                }

            }



            TestRepositories();

            TestReportService();

            // RawOperation();

            Console.WriteLine("Welcome to use RepoDB, the fastest ROM in the world!");
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

        private static void TestReportService()
        {
            //IDbContext context = new Repositories.SqlDbContext(ConnectionString);
            //var service = new ReportService(new Repositories.UnitOfWork(context));

            //var result = service.GetPatientByCareUnitID(3, new PageFilter { PagIndex = 0, PageSize = 100, });
        }



        private static void TestRepositories()
        {
            //IDbContext context = new Repositories.SqlDbContext(ConnectionString);
            //IUnitOfWork unitOfWork = new Repositories.UnitOfWork(context);
            //IPatientRepository patientRes = new PatientRepository(unitOfWork);

            //var patient = patientRes.GetByKey(2);

            //unitOfWork.ProcessWithTrans(() =>
            //{
            //    int maxId = (int)unitOfWork.ExecuteScalar("SELECT MAX(PatientID) FROM dbo.T_PATIENT");
            //    //Create
            //    var np = patientRes.Create(new Patient
            //    {
            //        FirstName = $"FirstName{maxId}",
            //        LastName = $"LastName{maxId}",
            //        MedRecordNumber = $"MRN{maxId}"
            //    });

            //    var newPatient = patientRes.GetByKey(np.ID);

            //    newPatient.MiddleInitial += "UPDATE";
            //    //Update
            //    patientRes.Update(newPatient);

            //    //Delete
            //   patientRes.Delete(newPatient);

            //   // throw new Exception("Rollback trans");
            //});

            //unitOfWork.ProcessWithoutTrans(() =>
            //{
            //    int maxId = (int)unitOfWork.ExecuteScalar("SELECT MAX(PatientID) FROM dbo.T_PATIENT");

            //    //Create
            //    var np = patientRes.Create(new Patient
            //    {
            //        FirstName = $"FirstName{maxId}",
            //        LastName = $"LastName{maxId}",
            //        MedRecordNumber = $"MRN{maxId}"
            //    });

            //    var newPatient = patientRes.GetByKey(np.ID);
            //    newPatient.MiddleInitial += "UPDATE";
            //    //Update
            //    patientRes.Update(newPatient);

            //   // Delete
            //    patientRes.Delete(newPatient);

            //});


        }

    }
}
