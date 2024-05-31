using Demo.Data.Models;
using Z.Dapper.Plus;

namespace Demo.Data.DapperRepository.Mappers
{
    /// <summary>
    ///https://dapper-plus.net/map
    /// Dapper Plus Mapper allows mapping the conceptual model (Entity) with the storage model (Database).
    /// </summary>
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

    /*
   Name	        Description
   Identity	    Sets column(s) which the database generates value. The value is outputted from the destination table (insert and merge action).
   Ignore	        Sets column(s) to ignore.
   Key	            Sets column(s) to use for Primary Key (update, delete, and merge action).
   Map	            Sets column(s) to input to the destination table.
   MapValue	    Sets value to map to the destination table.
   MapWithOptions	Sets column(s) to input to the destination table with options.
   Output	        Sets column(s) to output from the destination table (insert, update, and merge action).
   Table	        Sets the destination table or view name (including schema).
   AutoMap	        Set if the auto-mapping should be used.

    DapperPlusManager.Entity<Order>()
     .Identity(x => x.OrderID);

     .Ignore(x => x.Column1);

     .Key(order => order.OrderID);
     .Key(order => new { order.ApplicationID, order.OrderCode });    //Composite Key

     .Map(order => order.TotalQuantity, "qty").Map(order => order.TotalPrice, "TotalPrice");
     .Map(order => order.TotalPrice / order.TotalQuantity, "AvgPrice2");

     .MapValue(2, "ConstantColumn2");
      var constantValue = 2;
	 .MapValue(constantValue, "ConstantColumn2");

     .MapWithOptions(x => x.ModifiedDate, options =>
	    {
		    options.FormulaInsert = "GETDATE()";
		    options.FormulaUpdate = "GETDATE()";
	    })
	 .AutoMap();

      //Output single column
     .Output(order => order.Column1);
     //Output many columns
     .Output(order => new { order.Column1, order.Column2, order.Column3 });

     .Table("zzz.customers");

     .Map(order => new { order.TotalPrice }).AutoMap();
    */
}
