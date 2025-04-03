namespace MVC1.Models;

public class Category
{
    public int ID { get; set; }
    public string Name { get; set; }
    public string description { get; set; }
    public ICollection<Product> Products { get; set; }

}