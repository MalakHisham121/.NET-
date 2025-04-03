namespace MVC1.Models;

public class Product
{
    public int ID { get; set; }
    public string name { get; set; }
    public decimal price { get; set; }
    public string description { get; set; }
    public string imageName { get; set; }
    public int CategoryId { get; set; }
    public Category Category { get; set; }
    public ICollection<OrderItem> OrderItems { get; set; }
}