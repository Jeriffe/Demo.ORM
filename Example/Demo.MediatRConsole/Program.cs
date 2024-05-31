
using Demo.Application;
using Demo.Application.Patients.Command;
using Demo.DTOs;
using Demo.MediatRConsole;
using MediatR;

Console.OutputEncoding = System.Text.Encoding.Unicode;


CallAppService();

Console.ReadLine();

static void CallAppService()
{
    
    var mediatR = DIResolver.Resolve<IMediator>();

    TestOrderCache(mediatR);

    //TestOrder(mediatR);

    //TestPatient(mediatR);
}

static void TestOrderCache(IMediator mediatR)
{
    var items = mediatR.Send(new GetOrdersQuery() { PageFilter = new Demo.Infrastructure.PageFilter { } }).Result;

    var index = 1;
    foreach (var item in items)
    {
        var updatedPatient = mediatR.Send(new GetOrderQuery() { Id = item.Id }).Result;

        updatedPatient = mediatR.Send(new GetOrderQuery() { Id = item.Id }).Result;

        if (index++ > 1)
        {
            break;
        }
    }
}

static void TestOrder(IMediator mediatR)
{
    var items = mediatR.Send(new GetOrdersQuery() { PageFilter = new Demo.Infrastructure.PageFilter { } }).Result;

    if (items.Count > 0)
    {
        var updatedPatient = mediatR.Send(new GetOrderQuery() { Id = items[0].Id }).Result;

    }

}
static void TestPatient(IMediator mediatR)
{
    var patients = mediatR.Send(new GetPatientsQuery() { PageFilter = new Demo.Infrastructure.PageFilter { } }).Result;


    //Create
    var dtoP = new Patient
    {
        FirstName = $"FirstNamemaxId",
        LastName = $"LastNamemaxId",
        MedRecordNumber = $"MRNmaxId",
        BirthDate = DateTime.Now,
        DisChargeDate = DateTime.Now,
    };

    var newPatient = mediatR.Send(new CreatePatientCommand { Patient = dtoP }).Result;

    newPatient.MiddleInitial = "BaseAppSvc";

    mediatR.Send(new UpdatePatientCommand { Patient = newPatient });


    var updatedPatient = mediatR.Send(new GetPatientQuery() { Id = newPatient.PatientId }).Result;

    mediatR.Send(new DeletePatientCommand { Id = newPatient.PatientId });
}