using Demo.Data.Models;
using RepoDb;

namespace Demo.Data.RepoDBRepository
{
    public class FluentMappers
    {
        static FluentMappers()
        {
        }

        public static void Initialize()
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
