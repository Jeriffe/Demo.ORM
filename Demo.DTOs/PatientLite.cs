namespace Demo.DTOs
{
    internal class PatientLite
    {
        public int PatientId { get; set; }
        public string MedRecordNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? PatientType { get; set; }

        public virtual string FullName
        {
            get
            {
                return $"{FirstName}, {LastName}";
            }
        }
    }
}
