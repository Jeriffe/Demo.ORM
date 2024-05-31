using AutoMapper;
using Demo.Data.Models;
using Demo.DTOs;
using Demo.Infrastructure;
using Microsoft.Extensions.Logging;
using System;

namespace Demo.Services
{

    public interface IPatientSvc : IAppService<Patient>
    {
        void TransTest();
    }
    public class PatientService : BaseAppService<TPatient, Patient>, IPatientSvc
    {
        public PatientService(IUnitOfWork unitOfWork, IRepository<TPatient> repository, IMapper mapper, ILogger<IAppService<Patient>> logger )
            : base(unitOfWork, repository, mapper,logger)
        {
        }

        public void TransTest()
        {

            logger?.LogInformation("Start  PatientService.TransTest......");
            var plist = GetAll(null);

            //Use app service
            unitOfWork.ProcessByTrans(() =>
            {
                int maxId = (int)unitOfWork.ExecuteRawScalar("SELECT MAX(PatientID) FROM dbo.T_PATIENT");
                //Create
                var dtoP = new Patient
                {
                    FirstName = $"FirstName{maxId}",
                    LastName = $"LastName{maxId}",
                    MedRecordNumber = $"MRN{maxId}",
                    BirthDate = DateTime.Now,
                    DisChargeDate = DateTime.Now,
                    SiteId = 999,
                };
                dtoP = Create(dtoP);
                dtoP.MiddleInitial += "BaseAppSvc";
                Update(dtoP);
                var updatedP = GetSingle(dtoP.PatientId);

                // throw new Exception("Rollback trans");
            });

            int maxxId = (int)unitOfWork.ExecuteRawScalar("SELECT MAX(PatientID) FROM dbo.T_PATIENT");
            var pppp = GetSingle(maxxId);
            Delete(pppp);
        }
    }
}
