using Autofac;
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
using Serilog;
using Microsoft.Extensions.Logging;
using Serilog.Core;
namespace Demo.NETConsole
{
    public class DIResolver
    {
        private static IContainer container;
        private static IConfigurationRoot configuration;

        static DIResolver()
        {
            var services = new ServiceCollection();

            ConfigureLogger(services);

            ConfigureServices(services);

            // Initialize Autofac
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
            RegisterInstances(containerBuilder);

            // Creating a new AutofacServiceProvider makes the container
            // available to your app using the Microsoft IServiceProvider
            // interface so you can use those abstractions rather than
            // binding directly to Autofac.
            container = containerBuilder.Build();
        }
        static void ConfigureServices(IServiceCollection services)
        {
            //use :NuGet\Install-Package System.Configuration.ConfigurationManager  to get ConnStr from app.config
            var connstr = System.Configuration.ConfigurationManager.ConnectionStrings["DB"].ConnectionString;

            services.AddScoped<IDbContext>(c => new SqlDbContext(connstr));

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddAutoMapper(typeof(MappingProfile));

            Console.WriteLine($"ConnectionString={connstr}");
        }

        private static void ConfigureLogger(ServiceCollection services)
        {
            //https://github.com/serilog/serilog-sinks-file

            ////Serilog.Settings.AppSettings
            /////https://github.com/serilog/serilog-settings-appsettings
            {
                Log.Logger = new LoggerConfiguration()
                   .ReadFrom.AppSettings()
                   //     .WriteTo.Console()
                   ////RollingInterval.Day --> log20240531.txt
                   // .WriteTo.File(@"Logs\log.txt", rollingInterval: RollingInterval.Day, retainedFileCountLimit: 10)
                   .CreateLogger();
            }

            //Serilog.Settings.Configuration 
            //https://github.com/serilog/serilog-settings-configuration
            //{
            //    var configuration = new ConfigurationBuilder()
            //        .SetBasePath(Directory.GetCurrentDirectory())
            //        .AddJsonFile("appsettings.json")
            //        //  .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", true)
            //        .Build();

            //    Log.Logger = new LoggerConfiguration()
            //        .ReadFrom.Configuration(configuration)
            //        .CreateLogger();
            //}
            //This is very important to bridge autofac with Microsoft.Host.DependencyInjection
            services.AddLogging(configure => configure.AddSerilog(Log.Logger));
            //services.Configure<AppSettings>(configuration.GetSection("AppSettings"));

            //services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
        }
        private static void RegisterInstances(ContainerBuilder containerBuilder)
        {
            var connstr = configuration.GetConnectionString("DB");
            //containerBuilder.RegisterType<DbContextOfDapper>()
            //                .As<IDbContext>()
            //                .WithParameter("connectionString", connstr);

            containerBuilder.RegisterType<GenericRepository<TPatient>>().As<IRepository<TPatient>>();
            containerBuilder.RegisterType<GenericRepository<TOrder>>().As<IRepository<TOrder>>();
            containerBuilder.RegisterType<GenericRepository<TOrderItem>>().As<IRepository<TOrderItem>>();


            containerBuilder.RegisterType<PatientService>().As<IPatientSvc>();
            containerBuilder.RegisterType<OrderSvc>().As<IOrderSvc>();

        }

        public static T Resolve<T>(string name) where T : class
        {
            return container.ResolveNamed<T>(name);
        }

        public static T Resolve<T>() where T : class
        {
            return container.Resolve<T>();
        }

        public void Dispose()
        {
            if (container != null)
            {
                container.Dispose();
            }
        }
    }
}
