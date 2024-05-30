using System;


namespace Demo.Data.Models
{

    public class TOrderItem {
        public virtual long Id { get; set; }
        public virtual long OrderId { get; set; }
        public virtual long ProductId { get; set; }
        public virtual double Price { get; set; }
        public virtual DateTime CreateDate { get; set; }
        public virtual string Description { get; set; }
        public virtual TOrder Order { get; set; }
        public virtual TProduct Product { get; }

    }
}
