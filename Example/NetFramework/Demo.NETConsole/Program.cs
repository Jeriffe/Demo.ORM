using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;
using System.Threading;
using Demo.Services;
using Demo.Data.Models;
using Demo.DTOs;

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
            var service = DIResolver.Resolve<IAppService<TPatient, Patient>>();

           var result= service.GetAll(new Infrastructure.PageFilter { });

        }

        static async void CallHostedService()
        {
            var service = DIResolver.Resolve<IHostedService>();

            await service.StartAsync(new CancellationToken());
        }
    }
}
