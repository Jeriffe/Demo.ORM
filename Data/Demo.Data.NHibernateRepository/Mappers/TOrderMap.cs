using Demo.Data.Models;
using FluentNHibernate.Mapping;


namespace Demo.Data.NHibernateRepository
{

    public class TOrderMap : ClassMap<TOrder>
    {
        public TOrderMap()
        {
            Schema("dbo");
            Table("T_Order");
            Id(x => x.Id);

            Map(x => x.CustomerID);
            Map(x => x.CreateDate);
            Map(x => x.Description);
            Map(x => x.TotalPrice);
        }
    }
}
