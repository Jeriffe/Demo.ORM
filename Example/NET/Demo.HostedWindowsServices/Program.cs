using Demo.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using System;
using Topshelf;
using Topshelf.Extensions.Hosting;

namespace Demo.HostedWindowsServices
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
             .MinimumLevel.Debug()
             .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
             .Enrich.FromLogContext()
             .WriteTo.Console()
             .WriteTo.File(@"log\log.txt", rollingInterval: RollingInterval.Day)
             .CreateLogger();


            Console.OutputEncoding = System.Text.Encoding.Unicode;


            var builder = CreateHostBuilder(args);

            RunWindowsServiceWithHost(builder);

            Console.ReadLine();
        }
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(args)
                 .ConfigureServices(services =>
                 {

                     services.AddHostedService<SingleHostService>();

                     services.AddSingleton<IProcessor, ServiceProcessor>();

                     services.AddSingleton<ILoopTimer, LoopHostService>();

                 })
                 //NuGet\Install-Package Serilog.Extensions.Hosting -Version 8.0.0
                 .UseSerilog((context, services, loggerConfiguration) => loggerConfiguration
                //Install-Package Serilog.Settings.AppSettings
                .ReadFrom.AppSettings()
                //NuGet\Install-Package Serilog.Extensions.Hosting -Version 8.0.0
                .ReadFrom.Services(services)
                .Enrich.FromLogContext()
                .WriteTo.Console());


        private static void RunWindowsServiceWithHost(IHostBuilder builder)
        {
            //NuGet\Install-Package Topshelf.Extensions.Hosting -Version 0.4.0
            builder.RunAsTopshelfService(hc =>
            {
                hc.SetServiceName("GenericHostInTopshelf");
                hc.SetDisplayName("Generic Host In Topshelf");
                hc.SetDescription("Runs a generic host as a Topshelf service.");
            });
        }
    }
}
