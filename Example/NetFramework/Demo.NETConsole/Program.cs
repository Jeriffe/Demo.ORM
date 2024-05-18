using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;
using System.Threading;
using Demo.Services;

namespace Demo.NETConsole
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.Unicode;

            //Host Mode
            //StartupHost.InitializeHost(args, HostType.NewHost);

            //DI Mode
            CallHostedService();

            CallAppService();

            Console.ReadLine();
        }

        private static void CallAppService()
        {
            var service = DependencyInjectionResolver.Resolve<IPatientService>();

           var result= service.GetAll(new Infrastructure.PageFilter { });

        }

        static async void CallHostedService()
        {
            var service = DependencyInjectionResolver.Resolve<IHostedService>();

            await service.StartAsync(new CancellationToken());
        }
    }
}
