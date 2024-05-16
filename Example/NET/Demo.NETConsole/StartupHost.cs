using Autofac.Extensions.DependencyInjection;
using Autofac;
using Demo.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Demo.NETConsole
{
    public enum HostType
    {
        NewHost,
        ExtensionHost,
    }
    public class StartupHost
    {
        public static void InitializeHost(string[] args, HostType hostType = HostType.ExtensionHost)
        {
            switch (hostType)
            {
                case HostType.NewHost:
                    new Startup_New().BuildHost(args);
                    break;
                case HostType.ExtensionHost:
                default:
                    new Startup_Extension().BuildHost(args);
                    break;

            }
        }
        public class Startup_New
        {

            public void BuildHost(string[] args)
            {
                new HostBuilder().ConfigureServices(services =>
              {
                  services.AddSingleton<ILoopTimer, LoopHostService>();
                  services.AddSingleton<IHostedService, SingleService>();
              }).Build().Run();
            }
        }
        public class Startup_Extension
        {

            public void BuildHost(string[] args)
            {
                //  Host.CreateDefaultBuilder(args)
                //    .ConfigureServices(services =>
                //        {
                //            services.AddSingleton<ILoopTimer, LoopHostService>();
                //            services.AddSingleton<IHostedService, SingleService>();
                //        })
                //    .Build().Run();

                var builder = Host.CreateApplicationBuilder(args);

                builder.Services.AddSingleton<IHostedService, SingleService>();
                builder.Services.AddSingleton<ILoopTimer, LoopHostService>();

                builder.Build().Run();

              
            }
        }

    }
}
