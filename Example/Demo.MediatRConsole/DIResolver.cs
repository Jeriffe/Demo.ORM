using AutoMapper.Configuration;
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
        private static string ConnStr = string.Empty;

        static DIResolver()
        {
            _serviceProvider = BuildServiceProvider();
        }

        private static IServiceProvider BuildServiceProvider()
        {

            var services = new ServiceCollection();

            services.AddAutoMapper(typeof(MappingProfile));

            services.AddValidatorsFromAssembly(typeof(CreatePatientCommandValidator).Assembly);

            ConfigureLogger(services);

            ConfgirureConfiguration(services);

            ConfigureMediatR(services);

            ConfigureCache(services);

            ConfigureServices(services);

            return services.BuildServiceProvider();
        }

        private static void ConfgirureConfiguration(IServiceCollection services)
        {
            ////NuGet\Install-Package Microsoft.Extensions.Configuration
            /////public class ConfigurationBuilder : IConfigurationBuilder
            // var configuration = new ConfigurationBuilder()
            //              //NuGet\Install-Package Microsoft.Extensions.Configuration.Json
            //              .AddJsonFile("appsettings.json")
            //              .Build();
            // var connectionString = configuration.GetConnectionString("DB");
            //services.Configure<CacheSettings>(configuration.GetSection("CacheSettings"));

            //public sealed class ConfigurationManager : IConfigurationManager, IConfiguration, IConfigurationBuilder, IConfigurationRoot, IDisposable
            var configMgr = new Microsoft.Extensions.Configuration.ConfigurationManager();
            configMgr.AddJsonFile("appsettings.json");

            var connectionString = configMgr.GetConnectionString("DB");

            //NuGet\Install-Package Microsoft.Extensions.Options
            services.AddOptions()
                    .Configure<CacheSettings>(configMgr.GetSection("CacheSettings"));

            //use :NuGet\Install-Package System.Configuration.ConfigurationManager  to get ConnStr from app.config
            ConnStr = System.Configuration.ConfigurationManager.ConnectionStrings["DB"].ConnectionString;


            Log.Logger.Information($"ConnectionString={ConnStr}");
        }

        private static void ConfigureMediatR(IServiceCollection services)
        {
            services.AddMediatR(typeof(Application.GetPatientsQuery));

            //services.AddTransient(typeof(IRequestExceptionHandler<,>), typeof(RequestUnhandledExceptionHandler<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(CachingBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(AuthorizationBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        }

        private static void ConfigureCache(IServiceCollection services)
        {
            //services.Configure<CacheSettings>(config.GetSection("CacheSettings"));

            services.AddDistributedMemoryCache();

            ////Install-Package Microsoft.Extensions.Caching.StackExchangeRedis
            //services.AddStackExchangeRedisCache(options =>
            //{
            //    options.Configuration = "localhost:4455";
            //});

            //Install-Package Microsoft.Extensions.Caching.SqlServer -Version 8.0.0

            /*We need to create the cache table before we use it, here is an example by "dotnet sql-cache " command,
             * BTW, there are 3 parameters,  
                 * 1: ConnectionString; 
                 * 2:schema name; 
                 * 3: Cache Table Name
             
                dotnet tool install --global dotnet-sql-cache
                dotnet sql-cache create "Your ConnectionString;" dbo SqlServerCache


            *The created script of the cache table is as follows: 
                CREATE TABLE [dbo].[SqlServerCache]
                (
	                [Id]                            NVARCHAR(449)       NOT NULL,
	                [Value]                         VARBINARY(MAX)      NOT NULL,
	                [ExpiresAtTime]                 DATETIMEOFFSET(7)   NOT NULL,
	                [SlidingExpirationInSeconds]    BIGINT              NULL,
	                [AbsoluteExpiration]            DATETIMEOFFSET(7)   NULL,

	                PRIMARY KEY CLUSTERED ([Id] ASC)
                )
            */
            //services.AddDistributedSqlServerCache(options =>
            //{
            //    options.ConnectionString = ConnStr;
            //    options.SchemaName = "dbo";
            //    options.TableName = "SqlServerCache";
            //});

        }
        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IDbContext>(c => new SqlDbContext(ConnStr));
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
