using System;
using System.Text;
using System.Collections.Generic;


namespace Demo.DTOs
{

    public class Order
    {
        public long Id { get; set; }
        public Customer TCustomer { get; set; }
        public DateTime? CreateDate { get; set; }
        public string Description { get; set; }
        public double? TotalPrice { get; set; }

        public IList<OrderItem> OrderItems { get; set; }

    }
}
