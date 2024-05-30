using Demo.Data.Models;
using FluentNHibernate.Mapping;


namespace Demo.Data.NHibernateRepository
{


    public class TOrderitemMap : ClassMap<TOrderItem>
    {

        public TOrderitemMap()
        {
            Schema("dbo");
            Table("T_OrderItem");
            Id(x => x.Id);

            Map(x => x.OrderId);
            Map(x => x.ProductId);
            Map(x => x.Price);
            Map(x => x.CreateDate);
            Map(x => x.Description);
        }
    }
}
