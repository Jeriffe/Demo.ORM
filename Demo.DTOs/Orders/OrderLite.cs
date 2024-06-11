namespace Demo.DTOs.Orders
{
    public class OrderLite
    {
        public long OrderId { get; set; }
        public long CustomerID { get; set; }

        public string CustomerName { get; set; }

        public string Description { get; set; }

        public decimal TotalPrice { get; set; }
    }
}
