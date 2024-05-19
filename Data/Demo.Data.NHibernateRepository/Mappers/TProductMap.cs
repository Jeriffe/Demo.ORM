using Demo.Data.Models;
using FluentNHibernate.Mapping;


namespace Demo.Data.NHibernateRepository
{


    public class TProductMap : ClassMap<TProduct>
    {

        public TProductMap()
        {
            Schema("dbo");
            Table("T_Product");
            Id(x => x.Id);

            Map(x => x.Name);
            Map(x => x.Price);
            Map(x => x.Description);

        }
    }
}
