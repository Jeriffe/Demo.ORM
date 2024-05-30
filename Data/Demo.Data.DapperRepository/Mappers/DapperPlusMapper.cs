using Demo.Data.Models;
using Z.Dapper.Plus;

namespace Demo.Data.DapperRepository.Mappers
{
    internal class DapperPlusMapper
    {
        public static void Initialize()
        {
            //https://dapper-plus.net/map
            DapperPlusManager.Entity<TOrderItem>()
                             .Table("T_OrderItem")
                             .Identity(x => x.Id)
                             .Ignore(x => new { x.Product, x.Order });
        }

    }
}
