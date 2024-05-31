using Demo.DTOs;
using Demo.NETConsole;
using Demo.Services;

Console.OutputEncoding = System.Text.Encoding.Unicode;

CallAppService();

Console.ReadLine();

static void CallAppService()
{
    TestPatient();

    // TestOrder();

}

static void TestPatient()
{
    var service = DIResolver.Resolve<IPatientSvc>();

    var result = service.GetAll(new Demo.Infrastructure.PageFilter { });

    service.TransTest();
}

static void TestOrder()
{
    var service = DIResolver.Resolve<IOrderSvc>();

    var orders= service.GetAll(new Demo.Infrastructure.PageFilter { }).ToList();

    var singleOrder = service.GetSingle(orders[0]?.Id);

    //Create
    var createdOrder = new Demo.DTOs.Order
    {
        Customer = new Customer { Id = 4 },
        Description = $"Description:{888.88}",
        TotalPrice = 8888.88,
        OrderItems = new List<OrderItem>
                {
                    new OrderItem
                    {
                        ProductId=2,Price=99.99,
                        Description="Desc,P2,Price99.99",
                        CreateDate=DateTime.Now
                    },
                    new OrderItem
                    {
                        ProductId=10,Price=88.88,
                        Description="Desc,P10,Price88.88",
                        CreateDate=DateTime.Now
                    }
                }
    };
    createdOrder = service.Create(createdOrder);


    //Update
    var order = service.GetSingle(createdOrder.Id);
    order.TotalPrice = 999.99;
    order.OrderItems.RemoveAt(0);
    order.OrderItems[0].Price += 10;
    order.OrderItems.Add(new OrderItem
    {
        ProductId = 5,
        Price = 66.66,
        Description = "Desc,P5,Price66.66",
        CreateDate = DateTime.Now
    });

    service.Update(order);

    //Delete
    service.Delete(order);
}
