using Autofac;
using Autofac.Core;
using Autofac.Extensions.DependencyInjection;
using Demo.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Configuration;
using Microsoft.Extensions.Configuration;
using Demo.Data.Models;
using Demo.Services;
using Demo.DTOs;

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
            services.AddLogging();

            services.AddSingleton<IHostedService, SingleService>();


            //use :NuGet\Install-Package System.Configuration.ConfigurationManager  to get ConnStr from app.config
            var connstr = System.Configuration.ConfigurationManager.ConnectionStrings["DBSTR"].ConnectionString;

            services.AddScoped<IDbContext>(c => new Data.RepoDBRepository.SqlDbContext(connstr));
            services.AddScoped<IUnitOfWork, Data.RepoDBRepository.UnitOfWork>();


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
            containerBuilder.RegisterType<LoopHostService>().As<ILoopTimer>();


            containerBuilder.RegisterType<Data.RepoDBRepository.GenericRepository<TPatient>>().As<IRepository<Patient>>();

            containerBuilder.RegisterType<PatientService>().As<IPatientSvc>();


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
