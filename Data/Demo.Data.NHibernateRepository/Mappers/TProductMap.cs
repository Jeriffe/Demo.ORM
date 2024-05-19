using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Mapping.ByCode.Conformist;
using NHibernate.Mapping.ByCode;
using Demo.Data.NHibernateRepository;


namespace Demo.Data.NHibernateRepository {
    
    
    public class TProductMap : ClassMapping<TProduct> {
        
        public TProductMap() {
			Table("T_Product");
			Schema("dbo");
			Lazy(true);
			Id(x => x.Id, map => map.Generator(Generators.Identity));
			Property(x => x.Name, map => map.NotNullable(true));
			Property(x => x.Price, map => map.NotNullable(true));
			Property(x => x.Description);
            Bag(x => x.OrderItems, colmap => { colmap.Key(x => x.Column("ProductID")); colmap.Inverse(true); }, map => { map.OneToMany(); });

        }
    }
}
