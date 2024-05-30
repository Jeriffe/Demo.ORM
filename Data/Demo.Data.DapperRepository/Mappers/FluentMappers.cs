using Dapper.FluentMap;
using Dapper.FluentMap.Dommel;
using Dapper.FluentMap.Dommel.Mapping;
using Demo.Data.Models;
using Z.Dapper.Plus;

namespace Demo.Data.DapperRepository.Mappers
{
    public class FluentMappers
    {
        public static void Initialize()
        {
            FluentMapper.Initialize(config =>
            {
                config.AddMap(new PatientMap());
                config.AddMap(new OrderItemMap());
                config.AddMap(new OrderMap());
                
                config.ForDommel();
            });
        }
    }

    public class PatientMap : DommelEntityMap<TPatient>
    {

        public PatientMap()
        {
            ToTable("T_PATIENT", "dbo");
            Map(p => p.PatientId).IsIdentity().IsKey();
            // Map(p => p.PatientId).ToColumn("PatientId").IsIdentity().IsKey();
            // Map(p => p.MedRecNumber).ToColumn("MedRecNumber");
            // Map(p => p.FullName).Ignore();
        }
    }

    public class OrderMap : DommelEntityMap<TOrder>
    {

        public OrderMap()
        {
            ToTable("T_Order", "dbo");
            Map(p => p.Id).IsIdentity().IsKey();
        }
    }

    public class OrderItemMap : DommelEntityMap<TOrderItem>
    {

        public OrderItemMap()
        {
            ToTable("T_OrderItem", "dbo");
            Map(p => p.Id).IsIdentity().IsKey();
            Map(p => p.Order).Ignore();
            Map(p => p.Product).Ignore();

        }

        
    }
}
