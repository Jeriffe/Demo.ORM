using System;
using System.Text;
using System.Collections.Generic;


namespace Demo.Data.Models
{
    public class TPatient
    {
        public virtual int PatientId { get; set; }
        public virtual string MedRecNumber { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string MiddleInitial { get; set; }
        public virtual string LastName { get; set; }
        public virtual string Gender { get; set; }
        public virtual DateTime? BirthDate { get; set; }
        public virtual DateTime? DisChargeDate { get; set; }
        public virtual int? PatientType { get; set; }
        public virtual int? SiteId { get; set; }
    }
}
