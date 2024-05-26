﻿using Autofac;
using Autofac.Extensions.DependencyInjection;
using Demo.Data.Models;

using Demo.Data.DapperRepository;
//using Demo.Data.NHibernateRepository;
//using Demo.Data.RepoDBRepository;
using Demo.DTOs.Mapper;
using Demo.Infrastructure;
using Demo.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
namespace Demo.NETConsole
{
    public class DIResolver
    {
        private static IServiceProvider _serviceProvider;
        static DIResolver()
        {
            _serviceProvider = new Startup_Autofac().BuildServiceProvider();
        }

        public static T Resolve<T>()
        {
            return _serviceProvider.GetService<T>();
        }
        public static T ResolveNamed<T>(string name)
        {
            return _serviceProvider.GetKeyedService<T>(name);
        }
    }

    public class Startup_Autofac
    {
        public IConfiguration Configuration { get; }

        public IContainer container;
        public IServiceProvider BuildServiceProvider()
        {
            // The Microsoft.Extensions.DependencyInjection.ServiceCollection
            // has extension methods provided by other .NET Core libraries to
            // register services with DI.
            var services = new ServiceCollection();

            // The Microsoft.Extensions.Logging package provides this one-liner
            // to add logging services.
            services.AddLogging(config =>
            {
                config.AddDebug();
                config.AddConsole();
            });


            var connstr = Configuration.GetConnectionString("DB");

            //use :NuGet\Install-Package System.Configuration.ConfigurationManager  to get ConnStr from app.config
            connstr = System.Configuration.ConfigurationManager.ConnectionStrings["DB"].ConnectionString;

            services.AddScoped<IDbContext>(c => new SqlDbContext(connstr));
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddAutoMapper(typeof(MappingProfile));

            Console.WriteLine($"ConnectionString={connstr}");

            var containerBuilder = new ContainerBuilder();

            // Once you've registered everything in the ServiceCollection, call
            // Populate to bring those registrations into Autofac. This is
            // just like a foreach over the list of things in the collection
            // to add them to Autofac.
            containerBuilder.Populate(services);

            // Make your Autofac registrations. Order is important!
            // If you make them BEFORE you call Populate, then the
            // registrations in the ServiceCollection will override Autofac
            // registrations; if you make them AFTER Populate, the Autofac
            // registrations will override. You can make registrations
            // before or after Populate, however you choose.

            containerBuilder.RegisterType<GenericRepository<TPatient>>().As<IRepository<TPatient>>();
            containerBuilder.RegisterType<GenericRepository<TOrder>>().As<IRepository<TOrder>>();


            containerBuilder.RegisterType<PatientService>().As<IPatientSvc>();
            containerBuilder.RegisterType<OrderSvc>().As<IOrderSvc>();


            // Creating a new AutofacServiceProvider makes the container
            // available to your app using the Microsoft IServiceProvider
            // interface so you can use those abstractions rather than
            // binding directly to Autofac.
            container = containerBuilder.Build();
            var serviceProvider = new AutofacServiceProvider(container);


            return serviceProvider;
        }


    }
}