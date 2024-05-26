using Demo.Data.Models;
using FluentNHibernate.Mapping;


namespace Demo.Data.NHibernateRepository
{
    public class FluentMappers
    {
        public static void Initialize()
        {
        }
    }

    public class TPatientMap : ClassMap<TPatient>
    {
        public TPatientMap()
        {
            Schema("dbo");
            Table("T_PATIENT");
            Id(x => x.PatientId);

            Map(x => x.MedRecNumber);
            Map(x => x.FirstName);
            Map(x => x.MiddleInitial);
            Map(x => x.LastName);
            Map(x => x.Gender);
            Map(x => x.BirthDate);
            Map(x => x.DisChargeDate);
            Map(x => x.PatientType);
            Map(x => x.SiteId);
        }
    }
}
