using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Mapping.ByCode.Conformist;
using NHibernate.Mapping.ByCode;
using Demo.Data.NHibernateRepository;


namespace Demo.Data.NHibernateRepository {
    
    
    public class TPatientMap : ClassMapping<TPatient> {
        
        public TPatientMap() {
			Table("T_PATIENT");
			Schema("dbo");
			Lazy(true);
			Id(x => x.PatientId, map => map.Generator(Generators.Identity));
			Property(x => x.MedRecNumber);
			Property(x => x.FirstName);
			Property(x => x.MiddleInitial);
			Property(x => x.LastName);
			Property(x => x.Gender);
			Property(x => x.BirthDate);
			Property(x => x.DisChargeDate);
			Property(x => x.PatientType);
			Property(x => x.SiteId);
        }
    }
}
