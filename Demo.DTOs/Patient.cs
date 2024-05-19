using System;


namespace Demo.DTOs
{

    public class Patient
    {
        public int PatientId { get; set; }
        public string MedRecordNumber { get; set; }
        public string FirstName { get; set; }
        public string MiddleInitial { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public DateTime? BirthDate { get; set; }
        public DateTime? DisChargeDate { get; set; }
        public int? PatientType { get; set; }
        public int? SiteId { get; set; }

        public virtual string FullName
        {
            get
            {
                return $"{FirstName}, {LastName}";
            }
        }
    }
}
