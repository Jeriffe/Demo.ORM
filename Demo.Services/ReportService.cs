using Demo.DTOs;
using Demo.Infrastructure;
using Microsoft.Extensions.Logging;
using System.Data;

namespace Demo.Services
{
    public class ReportService
    {
        private IUnitOfWork unitOfWork;
        private ILogger<ReportService> logger;
        public ReportService(IUnitOfWork unitOfWork, ILogger<ReportService> logger)
        {
            this.unitOfWork = unitOfWork;
        }

        public DataTable GetPatientByCareUnitID(int careUnitID, PageFilter pageFilter)
        {
            logger?.LogInformation("Start ReportService.GetPatientByCareUnitID......");


            var sql = @"
    SELECT 
	     P.PatientID
		,P.FirstName
		,P.LastName
		,P.BirthDate
		,P.Gender
		,P.MedRecNumber
		,V.AccountNumber
		,V.CareUnitID
		,V.Room
		,V.Bed
	FROM dbo.T_PATIENT AS P
	INNER JOIN dbo.T_VISIT AS V ON P.PatientID = V.PatientID
	WHERE CareUnitID=@CareUnitID";

            var table = unitOfWork.ExecuteRawSql(sql, parameters: new RawParameter { Name = "@CareUnitID", Value = careUnitID });

            return table;
        }
    }
}