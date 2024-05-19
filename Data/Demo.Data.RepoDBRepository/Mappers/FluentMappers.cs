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
            var patienMapper = FluentMapper.Entity<TPatient>();
            patienMapper
            .Table("dbo.T_PATIENT")
            .Primary(e => e.PatientId)
            .Identity(e => e.PatientId);
            // .Column(e=>e.PatientId, "PatientId")
        }
    }
}
