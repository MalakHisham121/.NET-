namespace MVC1.Models;

public class OrderItem
{
    public int id { get; set; }
    public int Orderid { get; set; }
    public int Productid { get; set; }
    public int quantity { get; set; }
    public decimal price { get; set; }
    private Order order { get; set; }
    public Product product{ get; set; }
}