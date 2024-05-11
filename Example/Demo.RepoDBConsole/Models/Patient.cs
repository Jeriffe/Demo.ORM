
using System;

namespace Demo.RepoDBConsole.Models
{
    public class Patient
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string MiddleInitial { get; set; }
        public string LastName { get; set; }

        public DateTime? BirthDate { get; set; }
        public string Gender { get; set; }
        public string MedRecordNumber { get; set; }
        public DateTime? DisChargeDate { get; set; }
        public uint PatientType { get; set; }
        public int SiteID { get; set; }

        public string FullName
        {
            get
            {
                return $"{FirstName}, {LastName}";
            }
        }
    }
}
