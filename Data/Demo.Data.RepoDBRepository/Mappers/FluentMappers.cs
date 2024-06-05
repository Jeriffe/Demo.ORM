using Demo.Data.Models;
using RepoDb;

namespace Demo.Data.RepoDBRepository
{
    public class FluentMappers
    {
        public static string Sql_Schema = "dbo";

        static FluentMappers()
        {
        }

        public static void Initialize()
        {
            var ownerPrefix = string.Empty;
            if (!string.IsNullOrWhiteSpace(Sql_Schema))
            {
                ownerPrefix = $"{Sql_Schema}.";
            }
            var patienMapper = FluentMapper.Entity<TPatient>();

            patienMapper
            .Table($"{ownerPrefix}T_PATIENT")
            .Primary(e => e.PatientId)
            .Identity(e => e.PatientId)
            .DbType(e => e.BirthDate, System.Data.DbType.DateTime2)
            .DbType(e => e.DisChargeDate, System.Data.DbType.DateTime2);
            // .Column(e=>e.PatientId, "PatientId")

            var orderMapper = FluentMapper.Entity<TOrder>();
            orderMapper.Table($"{ownerPrefix}T_Order")
                        .Primary(e => e.Id)
                        .Identity(e => e.Id)
                        .DbType(e => e.CreateDate, System.Data.DbType.DateTime2);

            var orderItemMapper = FluentMapper.Entity<TOrderItem>();
            orderItemMapper.Table($"{ownerPrefix}T_OrderItem")
                        .Primary(e => e.Id)
                        .Identity(e => e.Id)
                        .DbType(e => e.CreateDate, System.Data.DbType.DateTime2);

            var customerMapper = FluentMapper.Entity<TCustomer>();
            customerMapper.Table($"{ownerPrefix}T_Customer")
                        .Primary(e => e.Id)
                        .Identity(e => e.Id)
                        .DbType(e => e.Birthday, System.Data.DbType.DateTime2);


            var productMapper = FluentMapper.Entity<TProduct>();
            productMapper.Table($"{ownerPrefix}T_Product")
                        .Primary(e => e.Id)
                        .Identity(e => e.Id)
                        .DbType(e => e.Price, System.Data.DbType.Decimal);


        }
    }
}
