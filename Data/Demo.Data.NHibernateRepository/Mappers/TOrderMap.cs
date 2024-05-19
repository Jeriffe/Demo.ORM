using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Mapping.ByCode.Conformist;
using NHibernate.Mapping.ByCode;
using Demo.Data.NHibernateRepository;


namespace Demo.Data.NHibernateRepository {
    
    
    public class TOrderMap : ClassMapping<TOrder> {
        
        public TOrderMap() {
			Table("T_Order");
			Schema("dbo");
			Lazy(true);
			Id(x => x.Id, map => map.Generator(Generators.Identity));
			Property(x => x.CreateDate);
			Property(x => x.Description);
			Property(x => x.TotalPrice);
			ManyToOne(x => x.TCustomer, map => 
			{
				map.Column("CustomerID");
				map.NotNullable(true);
				map.Cascade(Cascade.None);
			});
            Bag(x => x.OrderItems, colmap => { colmap.Key(x => x.Column("OrderID")); colmap.Inverse(true); }, map => { map.OneToMany(); });

        }
    }
}
