using Demo.Data.Models;
using Demo.DBScripts;
using Demo.Infrastructure;
using System.Collections.Generic;
using System.Data;

namespace Demo.Data.RepoDBRepository
{

    public interface IPatientRepository : IRepoDBRepository<TPatient>
    {
        TPatient GetPatientByAccountNumber(string accountNumber);
        IEnumerable<TPatient> GetActivePatients(PageFilter pagFilter);
        IEnumerable<TPatient> GetPatientByCareUnitID(int careUnitID, PageFilter pagFilter);
    }

    public class PatientRepository : GenericRepository<TPatient>, IPatientRepository
    {
        public PatientRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

        public TPatient GetPatientByAccountNumber(string accountNumber)
        {
            var sql = ScriptsLoader.Get("PATIENT_SINGLE_BY_ACCT");

            //var patient = GetSingle(sql, CommandType.Text, new { AccountNumber = accountNumber, });

            //return patient;

            return null;

        }

        public IEnumerable<TPatient> GetPatientByCareUnitID(int careUnitID, PageFilter pagFilter)
        {
            var sql = ScriptsLoader.Get("PATIENT_QUERY_BY_CAREUNITID");

            var offSet = GetOffset(pagFilter.PagIndex, pagFilter.PageSize);
            var conditon = new
            {
                CareUnitID = careUnitID,
                Offset = offSet,
                PageSize = pagFilter.PageSize
            };

            //var patients = GetList(sql, CommandType.StoredProcedure, conditon);

            //return patients;
            return null;
        }

        public IEnumerable<TPatient> GetActivePatients(PageFilter pagFilter)
        {
            var sql = ScriptsLoader.Get("PATIENT_QUERY_BY_DISCHARGEDATE");

            var conditon = new
            {
                Offset = GetOffset(pagFilter.PagIndex, pagFilter.PageSize),
                PageSize = pagFilter.PageSize
            };

            //var patients = GetList(sql, CommandType.Text, conditon);

            //return patients;
            return null;

        }
    }
}
