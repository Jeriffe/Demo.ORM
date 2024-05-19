using Demo.Data.Models;
using FluentNHibernate.Mapping;

namespace Demo.Data.NHibernateRepository
{
    public class TCustomerMap : ClassMap<TCustomer>
    {
        public TCustomerMap()
        {
            Schema("dbo");
            Table("T_Customer");
            Id(x => x.Id);

            Map(x => x.Name);
            Map(x => x.Gender);
            Map(x => x.Birthday);
            Map(x => x.Phone);
            Map(x => x.Address);
        }
    }
}
