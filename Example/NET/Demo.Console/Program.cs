using Demo.Data.Models;
using Demo.DTOs;
using Demo.NETConsole;
using Demo.Services;

Console.OutputEncoding = System.Text.Encoding.Unicode;


CallAppService();

Console.ReadLine();

static void CallAppService()
{
    var service = DependencyInjectionResolver.Resolve<IAppService<TPatient, Patient>>();

    var result = service.GetAll(new Demo.Infrastructure.PageFilter { });

}
