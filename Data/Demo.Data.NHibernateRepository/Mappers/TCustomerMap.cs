using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;


namespace Demo.Data.NHibernateRepository
{


    public class TCustomerMap : ClassMapping<TCustomer> {
        
        public TCustomerMap() {
			Table("T_Customer");
			Schema("dbo");
			Lazy(true);
			Id(x => x.Id, map => map.Generator(Generators.Identity));
			Property(x => x.Name, map => map.NotNullable(true));
			Property(x => x.Gender, map => map.NotNullable(true));
			Property(x => x.Birthday);
			Property(x => x.Phone);
			Property(x => x.Address);
			Bag(x => x.Orders, colmap =>  { colmap.Key(x => x.Column("CustomerID")); colmap.Inverse(true); }, map => { map.OneToMany(); });
        }
    }
}
