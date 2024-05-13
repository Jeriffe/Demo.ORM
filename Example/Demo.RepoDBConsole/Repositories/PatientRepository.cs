﻿using Demo.DBScripts;
using Demo.Infrastructure;
using Demo.RepoDBConsole.Models;
using Demo.RepoDBConsole.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Demo.RepoDBConsole
{

    public interface IPatientRepository : IRepoDBRepository<Patient,int>
    {
        Patient GetPatientByAccountNumber(string accountNumber);
        IEnumerable<Patient> GetActivePatients(PageFilter pagFilter);
        IEnumerable<Patient> GetPatientByCareUnitID(int careUnitID, PageFilter pagFilter);
    }

    public class PatientRepository : GenericRepository<Patient>, IPatientRepository
    {
        public PatientRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

        public Patient GetPatientByAccountNumber(string accountNumber)
        {
            var sql = ScriptsLoader.Get("PATIENT_SINGLE_BY_ACCT");

            var patient = GetSingle(sql, CommandType.Text, new { AccountNumber = accountNumber, });

            return patient;
        }

        public IEnumerable<Patient> GetPatientByCareUnitID(int careUnitID, PageFilter pagFilter)
        {
            var sql = ScriptsLoader.Get("PATIENT_QUERY_BY_CAREUNITID");

            var offSet = GetOffset(pagFilter.PagIndex, pagFilter.PageSize);
            var conditon = new
            {
                CareUnitID = careUnitID,
                Offset = offSet,
                PageSize = pagFilter.PageSize
            };

            var patients = GetList(sql, CommandType.StoredProcedure, conditon);

            return patients;
        }

        public IEnumerable<Patient> GetActivePatients(PageFilter pagFilter)
        {
            var sql = ScriptsLoader.Get("PATIENT_QUERY_BY_DISCHARGEDATE");

            var conditon = new
            {
                Offset = GetOffset(pagFilter.PagIndex, pagFilter.PageSize),
                PageSize = pagFilter.PageSize
            };

            var patients = GetList(sql, CommandType.Text, conditon);

            return patients;
        }
    }
}
