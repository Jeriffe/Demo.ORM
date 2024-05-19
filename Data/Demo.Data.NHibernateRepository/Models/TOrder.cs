using System;
using System.Text;
using System.Collections.Generic;


namespace Demo.Data.NHibernateRepository
{

    public class TOrder
    {
        public virtual long Id { get; set; }
        public virtual TCustomer TCustomer { get; set; }
        public virtual DateTime? CreateDate { get; set; }
        public virtual string Description { get; set; }
        public virtual double? TotalPrice { get; set; }

        public virtual IList<TOrderitem> OrderItems { get; set; }

    }
}
