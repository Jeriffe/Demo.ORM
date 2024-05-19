using System;
using System.Text;
using System.Collections.Generic;


namespace Demo.Data.Models
{

    public class TCustomer
    {
        public TCustomer() { }
        public virtual long Id { get; set; }
        public virtual string Name { get; set; }
        public virtual bool Gender { get; set; }
        public virtual DateTime? Birthday { get; set; }
        public virtual string Phone { get; set; }
        public virtual string Address { get; set; }
        public virtual IList<TOrder> Orders { get; set; }
    }
}
