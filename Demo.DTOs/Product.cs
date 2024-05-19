using System.Collections.Generic;


namespace Demo.DTOs
{

    public class Product {
        public long Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }

        public IList<OrderItem> OrderItems { get;}
    }
}
