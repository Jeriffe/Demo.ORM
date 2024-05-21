using Demo.Infrastructure;
using System.Data;

namespace Demo.Services
{
    public class ReportService
    {
        private IUnitOfWork unitOfWork;
        public ReportService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public DataTable GetPatientByCareUnitID(int careUnitID, PageFilter pageFilter)
        {
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

            var table = unitOfWork.ExecuteRawSql(sql, new RawParameter { Name = "@CareUnitID", Value = careUnitID });

            return table;
        }
    }
}