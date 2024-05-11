using Demo.Infrastructure;
using RepoDb;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;

namespace Demo.RepoDBConsole
{
    public class ReportService
    {
        private IUnitOfWork<IDbContext> unitOfWork;
        public ReportService(IUnitOfWork<IDbContext> unitOfWork)
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

            var table = new DataTable();

            using (var connection = unitOfWork.Context.Connection)
            {
                using (var reader = connection.ExecuteReader(sql, new { CareUnitID = careUnitID }))
                {
                    table.Load(reader);
                }
            }

            return table;
        }
    }
}