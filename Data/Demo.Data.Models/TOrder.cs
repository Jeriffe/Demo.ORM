using System;
using System.Text;
using System.Collections.Generic;


namespace Demo.Data.Models
{

    public class TOrder
    {
        public virtual long Id { get; set; }

        public virtual long CustomerID { get; set; }

        public virtual DateTime? CreateDate { get; set; }
        public virtual string Description { get; set; }
        public virtual double? TotalPrice { get; set; }

        public virtual TCustomer TCustomer { get; set; }

        public virtual IList<TOrderItem> OrderItems { get; set; }

    }
}
