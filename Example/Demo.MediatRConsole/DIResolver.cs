using Demo.Data.Models;

//using Demo.Data.DapperRepository;
//using Demo.Data.NHibernateRepository;
using Demo.Data.RepoDBRepository;
using Demo.DTOs.Mapper;
using Demo.Infrastructure;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
namespace Demo.MediatRConsole
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
        public IServiceProvider BuildServiceProvider()
        {
            var services = new ServiceCollection();

            //use :NuGet\Install-Package System.Configuration.ConfigurationManager  to get ConnStr from app.config
          var  connstr = System.Configuration.ConfigurationManager.ConnectionStrings["DB"].ConnectionString;

            services.AddAutoMapper(typeof(MappingProfile));

            services.AddMediatR(typeof(Demo.Application.GetPatientQuery));

            services.AddScoped<IDbContext>(c => new SqlDbContext(connstr));
            services.AddScoped<IUnitOfWork,UnitOfWork>();

            services.AddScoped<IRepository<TPatient>, GenericRepository<TPatient>>();

            Console.WriteLine($"ConnectionString={connstr}");

            return services.BuildServiceProvider();
        }

       
    }
}
