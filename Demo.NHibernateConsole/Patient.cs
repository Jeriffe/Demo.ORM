
using System;

namespace Demo.Models
{
    public class Patient
    {
        public virtual int ID { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string MiddleInitial { get; set; }
        public virtual string LastName { get; set; }

        public virtual DateTime? BirthDate { get; set; }
        public virtual string Gender { get; set; }
        public virtual string MedRecordNumber { get; set; }
        public virtual DateTime? DisChargeDate { get; set; }
        public virtual int PatientType { get; set; }
        public virtual int SiteID { get; set; }

        public virtual string FullName
        {
            get
            {
                return $"{FirstName}, {LastName}";
            }
        }
    }
}
