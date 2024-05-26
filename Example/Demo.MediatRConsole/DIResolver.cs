﻿using Demo.Application.Common.Behaviours;
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
using Microsoft.Extensions.Logging;
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
            var connstr = System.Configuration.ConfigurationManager.ConnectionStrings["DB"].ConnectionString;

            services.AddLogging(config =>
               {
                   config.AddConsole();
               });

            services.AddAutoMapper(typeof(MappingProfile));

            services.AddValidatorsFromAssembly(typeof(CreatePatientCommandValidator).Assembly);


            services.AddMediatR(typeof(Application.GetPatientsQuery));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(AuthorizationBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));


            services.AddScoped<IDbContext>(c => new SqlDbContext(connstr));
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IRepository<TPatient>, GenericRepository<TPatient>>();

            Console.WriteLine($"ConnectionString={connstr}");

            return services.BuildServiceProvider();
        }


    }
}