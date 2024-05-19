using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Mapping.ByCode.Conformist;
using NHibernate.Mapping.ByCode;
using Demo.Data.NHibernateRepository;


namespace Demo.Data.NHibernateRepository {
    
    
    public class TOrderitemMap : ClassMapping<TOrderitem> {
        
        public TOrderitemMap() {
			Table("T_OrderItem");
			Schema("dbo");
			Lazy(true);
			Id(x => x.Id, map => map.Generator(Generators.Identity));
			Property(x => x.OrderId, map => map.NotNullable(true));
			Property(x => x.ProductId, map => map.NotNullable(true));
			Property(x => x.Price, map => map.NotNullable(true));
			Property(x => x.CreateDate, map => map.NotNullable(true));
			Property(x => x.Description);
            ManyToOne(x => x.Order, map =>
            {
                map.Column("OrderID");
                map.NotNullable(true);
                map.Cascade(Cascade.None);
            });
            ManyToOne(x => x.Product, map =>
            {
                map.Column("ProductID");
                map.NotNullable(true);
                map.Cascade(Cascade.None);
            });

        }
    }
}
