using Demo.NETConsole;
using Demo.Services;

Console.OutputEncoding = System.Text.Encoding.Unicode;


CallAppService();

Console.ReadLine();

static void CallAppService()
{
    var service = DIResolver.Resolve<IPatientSvc>();

    var result = service.GetAll(new Demo.Infrastructure.PageFilter { });

    service.TransTest();

}
