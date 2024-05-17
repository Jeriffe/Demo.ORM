using Demo.Data.Models;
using FluentNHibernate.Mapping;

namespace Demo.Data.NHibernateRepository
{
    public class FluentMappers
    {
        static FluentMappers()
        {
        }
    }

    public class PatientMap : ClassMap<Patient>
    {
        public PatientMap()
        {
            Id(x => x.ID,"PatientID");
            Map(x => x.MedRecordNumber, "MedRecNumber");
            Map(x => x.FirstName);
            Map(x => x.MiddleInitial);
            Map(x => x.LastName);
            Map(x => x.Gender);
            Map(x => x.BirthDate);
            Map(x => x.DisChargeDate);
            Map(x => x.PatientType);
            Map(x => x.SiteID);
            Table("T_PATIENT");
        }
    }
}
