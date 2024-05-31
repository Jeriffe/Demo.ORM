using Demo.Data.Models;
//using Demo.Data.DapperRepository;
//using Demo.Data.NHibernateRepository;
using Demo.Data.RepoDBRepository;
using Demo.DTOs.Mapper;
using Demo.Infrastructure;
using Demo.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using WindowsFormsLifetime;

namespace Demo.NETWinForm
{
    public class DIResolver
    {
        static DIResolver()
        {
            var builder = UseCreateApplicationBuilder();
            // var builder = UseWindowsFormsLifetime();

            ServiceProvider = builder.Services;
        }

        private static IHost UseWindowsFormsLifetime()
        {
            //WindowsFormsLifetime
            //https://github.com/alex-oswald/WindowsFormsLifetime
            var builder = Host.CreateApplicationBuilder();
            builder.UseWindowsFormsLifetime<MainForm>();

            ConfigureLogger(builder.Services);

            ConfigureServices(builder.Services);

            return builder.Build();
        }

        private static IHost UseCreateApplicationBuilder()
        {
            return Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    ConfigureLogger(services);

                    ConfigureServices(services);

                    services.AddTransient<MainForm>();
                }).Build();
        }

        static void ConfigureServices(IServiceCollection services)
        {
            //use :NuGet\Install-Package System.Configuration.ConfigurationManager  to get ConnStr from app.config
            var connstr = System.Configuration.ConfigurationManager.ConnectionStrings["DB"].ConnectionString;

            services.AddScoped<IDbContext>(c => new SqlDbContext(connstr));

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddAutoMapper(typeof(MappingProfile));

            services.AddScoped<IRepository<TPatient>, GenericRepository<TPatient>>();

            services.AddScoped<IRepository<TPatient>, GenericRepository<TPatient>>();
            services.AddScoped<IRepository<TOrder>, GenericRepository<TOrder>>();
            services.AddScoped<IRepository<TOrderItem>, GenericRepository<TOrderItem>>();


            services.AddScoped<IPatientSvc, PatientService>();
            services.AddScoped<IOrderSvc, OrderSvc>();

            Console.WriteLine($"ConnectionString={connstr}");
        }

        static void ConfigureLogger(IServiceCollection services)
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

        private static IServiceProvider ServiceProvider { get; set; }

        public static T Resolve<T>(string name) where T : class
        {
            return ServiceProvider.GetRequiredKeyedService<T>(name);
        }

        public static T Resolve<T>() where T : class
        {
            return ServiceProvider.GetRequiredService<T>();
        }
    }
}
