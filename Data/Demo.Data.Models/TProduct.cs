using System.Collections.Generic;


namespace Demo.Data.Models
{
    public class TProduct {
        public virtual long Id { get; set; }
        public virtual string Name { get; set; }
        public virtual decimal Price { get; set; }
        public virtual string Description { get; set; }

        public virtual IList<TOrderItem> OrderItems { get;}
    }
}
