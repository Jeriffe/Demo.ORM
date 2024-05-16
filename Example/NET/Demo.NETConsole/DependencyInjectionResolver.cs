using Autofac.Extensions.DependencyInjection;
using Autofac;
using Demo.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.NETConsole
{
    public class DependencyInjectionResolver
    {
        private static IServiceProvider _serviceProvider;
        static DependencyInjectionResolver()
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
        public IContainer container;
        public IServiceProvider BuildServiceProvider()
        {
            // The Microsoft.Extensions.DependencyInjection.ServiceCollection
            // has extension methods provided by other .NET Core libraries to
            // register services with DI.
            var serviceCollection = new ServiceCollection();

            // The Microsoft.Extensions.Logging package provides this one-liner
            // to add logging services.
            serviceCollection.AddLogging();

            serviceCollection.AddSingleton<IHostedService, SingleService>();

            var containerBuilder = new ContainerBuilder();

            // Once you've registered everything in the ServiceCollection, call
            // Populate to bring those registrations into Autofac. This is
            // just like a foreach over the list of things in the collection
            // to add them to Autofac.
            containerBuilder.Populate(serviceCollection);

            // Make your Autofac registrations. Order is important!
            // If you make them BEFORE you call Populate, then the
            // registrations in the ServiceCollection will override Autofac
            // registrations; if you make them AFTER Populate, the Autofac
            // registrations will override. You can make registrations
            // before or after Populate, however you choose.
            containerBuilder.RegisterType<LoopHostService>().As<ILoopTimer>();

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
