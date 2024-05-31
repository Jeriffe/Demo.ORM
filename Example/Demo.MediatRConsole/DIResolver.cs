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
using Microsoft.Extensions.DependencyInjection;
using Serilog;
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

            //use :NuGet\Install-Package System.Configuration.ConfigurationManager  to get ConnStr from app.config
            var connstr = System.Configuration.ConfigurationManager.ConnectionStrings["DB"].ConnectionString;

            services.AddAutoMapper(typeof(MappingProfile));

            services.AddValidatorsFromAssembly(typeof(CreatePatientCommandValidator).Assembly);

            services.AddMediatR(typeof(Application.GetPatientsQuery));

            ConfigureLogger(services);

            ConfigureServices(services, connstr);

            Log.Logger.Information($"ConnectionString={connstr}");

            return services.BuildServiceProvider();
        }

        private static void ConfigureServices(ServiceCollection services, string connstr)
        {
            //services.AddTransient(typeof(IRequestExceptionHandler<,>), typeof(RequestUnhandledExceptionHandler<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(AuthorizationBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));


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
