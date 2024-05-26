
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
