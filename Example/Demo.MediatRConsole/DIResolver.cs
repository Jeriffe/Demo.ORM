using Demo.Application.Common.Behaviours;
using Demo.Application.Patients.Command;
using Demo.Data.Models;

//using Demo.Data.DapperRepository;
//using Demo.Data.NHibernateRepository;
using Demo.Data.RepoDBRepository;
using Demo.DTOs.Mapper;
using Demo.Infrastructure;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System.Configuration;
namespace Demo.MediatRConsole
{
    public class DIResolver
    {
        private static IServiceProvider _serviceProvider;
        static DIResolver()
        {
            _serviceProvider = BuildServiceProvider();
        }

        private static IServiceProvider BuildServiceProvider()
        {
            var services = new ServiceCollection();

            services.AddAutoMapper(typeof(MappingProfile));

            services.AddValidatorsFromAssembly(typeof(CreatePatientCommandValidator).Assembly);

            ConfigureMediatR(services);

            ConfigureCache(services);

            ConfigureLogger(services);

            ConfigureServices(services);


            return services.BuildServiceProvider();
        }

        private static void ConfigureMediatR(ServiceCollection services)
        {
            services.AddMediatR(typeof(Application.GetPatientsQuery));

            //services.AddTransient(typeof(IRequestExceptionHandler<,>), typeof(RequestUnhandledExceptionHandler<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(CachingBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(AuthorizationBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        }

        private static void ConfigureCache(ServiceCollection services)
        {
            //services.Configure<CacheSettings>(config.GetSection("CacheSettings"));

            services.AddDistributedMemoryCache();

            ////Install-Package Microsoft.Extensions.Caching.StackExchangeRedis
            //services.AddStackExchangeRedisCache(options =>
            //{
            //    options.Configuration = "localhost:4455";
            //});
        }

        private static void ConfigureServices(ServiceCollection services)
        {
            //use :NuGet\Install-Package System.Configuration.ConfigurationManager  to get ConnStr from app.config
            var connstr = System.Configuration.ConfigurationManager.ConnectionStrings["DB"].ConnectionString;

            //NuGet\Install-Package Microsoft.Extensions.Configuration
            var configuration = new ConfigurationBuilder()
                            //NuGet\Install-Package Microsoft.Extensions.Configuration.Json
                            .AddJsonFile("appsettings.json")
                            .Build();

            var connectionString = configuration.GetConnectionString("DB");
           
            Log.Logger.Information($"ConnectionString={connstr}");


            services.AddScoped<IDbContext>(c => new SqlDbContext(connstr));
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IRepository<TPatient>, GenericRepository<TPatient>>();
            services.AddScoped<IRepository<TOrder>, GenericRepository<TOrder>>();
        }

        private static void ConfigureLogger(IServiceCollection services)
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

            services.AddLogging(config => config.AddSerilog(Log.Logger));
        }

        public static T Resolve<T>() where T : class
        {
            return _serviceProvider.GetService<T>();
        }
        public static T ResolveNamed<T>(string name) where T : class
        {
            return _serviceProvider.GetKeyedService<T>(name);
        }
    }
}
