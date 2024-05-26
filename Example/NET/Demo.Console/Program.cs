using Demo.NETConsole;
using Demo.Services;
using RepoDb.Enumerations;

Console.OutputEncoding = System.Text.Encoding.Unicode;


CallAppService();

Console.ReadLine();

static void CallAppService()
{
    TestPatient();

    TestOrder();

}

static void TestPatient()
{
    var service = DIResolver.Resolve<IPatientSvc>();

    var result = service.GetAll(new Demo.Infrastructure.PageFilter { });

    service.TransTest();
}

static void TestOrder()
{
    var service = DIResolver.Resolve<IOrderSvc>();

    var result = service.GetAll(new Demo.Infrastructure.PageFilter { }).ToList();

    var order=service.GetSingle(result[0]?.Id);
}