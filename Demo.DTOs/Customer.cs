using System;
using System.Text;
using System.Collections.Generic;


namespace Demo.DTOs
{

    public class Customer
    {
        public Customer() { }
        public long Id { get; set; }
        public string Name { get; set; }
        public bool Gender { get; set; }
        public DateTime? Birthday { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public IList<Order> Orders { get; set; }
    }
}
