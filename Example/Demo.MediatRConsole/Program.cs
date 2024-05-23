
using Demo.Application;
using Demo.MediatRConsole;
using MediatR;

Console.OutputEncoding = System.Text.Encoding.Unicode;


CallAppService();

Console.ReadLine();

static void CallAppService()
{

    var mediatR = DIResolver.Resolve<IMediator>();

    var patients = mediatR.Send(new GetPatientQuery() { PageFilter = new Demo.Infrastructure.PageFilter { } }).Result;
}
