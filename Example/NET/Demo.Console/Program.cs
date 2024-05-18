using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;
using System.Threading;
using Demo.Services;
using Demo.NETConsole;

Console.OutputEncoding = System.Text.Encoding.Unicode;

    //Host Mode
    //StartupHost.InitializeHost(args, HostType.NewHost);

    //DI Mode

    CallAppService();

    Console.ReadLine();

  static void CallAppService()
{
    var service = DependencyInjectionResolver.Resolve<IPatientService>();

    var result = service.GetAll(new Demo.Infrastructure.PageFilter { });

}
 