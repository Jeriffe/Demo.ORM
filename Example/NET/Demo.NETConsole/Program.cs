using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;
using System.Threading;

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
            CallService();
        }

        static async void CallService()
        {
            var service = DependencyInjectionResolver.Resolve<IHostedService>();

            await service.StartAsync(new CancellationToken());

            Console.ReadLine();
        }
    }
}
