using RepoDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.RepoDBConsole.Models.Mappers
{
    public class FluentMappers
    {
        static FluentMappers()
        {
        }

        internal static void Initialize()
        {
            var patienMapper = FluentMapper.Entity<Patient>();
            patienMapper
            .Table("dbo.T_PATIENT")
            .Primary(e => e.ID)
            .Identity(e => e.ID)
            .Column(e=>e.ID,"PatientID")
            .Column(e => e.MedRecordNumber, "[MedRecNumber]");
        }
    }
}
