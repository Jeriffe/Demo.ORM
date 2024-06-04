using Dapper.FluentMap;
using Dapper.FluentMap.Dommel;
using Dapper.FluentMap.Dommel.Mapping;
using Demo.Data.Models;

namespace Demo.Data.DapperRepository
{
    public class FluentMappers
    {

        public static string Sql_Schema = "dbo";
        public static void Initialize()
        {
            FluentMapper.Initialize(config =>
            {
                config.AddMap(new PatientMap());
                config.AddMap(new OrderItemMap());
                config.AddMap(new OrderMap());
                config.AddMap(new ProductItemMap());
                
                config.ForDommel();
            });
        }
    }

    public class PatientMap : DommelEntityMap<TPatient>
    {

        public PatientMap()
        {
            if (string.IsNullOrEmpty(FluentMappers.Sql_Schema))
            {
                ToTable("T_PATIENT");
            }
            else
            {
                ToTable("T_PATIENT", FluentMappers.Sql_Schema);
            }

            Map(p => p.PatientId)
                .IsIdentity()
                .IsKey()
                .SetGeneratedOption(  System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            // Map(p => p.PatientId).ToColumn("PatientId").IsIdentity().IsKey();
            // Map(p => p.MedRecNumber).ToColumn("MedRecNumber");
            // Map(p => p.FullName).Ignore();
        }
    }

    public class OrderMap : DommelEntityMap<TOrder>
    {

        public OrderMap()
        {
            if (string.IsNullOrEmpty(FluentMappers.Sql_Schema))
            {
                ToTable("T_Order");
            }
            else
            {
                ToTable("T_Order", FluentMappers.Sql_Schema);
            }

            
            Map(p => p.Id).IsIdentity().IsKey();
        }
    }

    public class OrderItemMap : DommelEntityMap<TOrderItem>
    {

        public OrderItemMap()
        {
            if (string.IsNullOrEmpty(FluentMappers.Sql_Schema))
            {
                ToTable("T_OrderItem");
            }
            else
            {
                ToTable("T_OrderItem", FluentMappers.Sql_Schema);
            }

           
            Map(p => p.Id).IsIdentity().IsKey();
            Map(p => p.Order).Ignore();
            Map(p => p.Product).Ignore();

        }
    }

    public class ProductItemMap : DommelEntityMap<TProduct>
    {

        public ProductItemMap()
        {
            if (string.IsNullOrEmpty(FluentMappers.Sql_Schema))
            {
                ToTable("T_Product");
            }
            else
            {
                ToTable("T_Product", FluentMappers.Sql_Schema);
            }


            Map(p => p.Id).IsIdentity().IsKey();

        }
    }
}
