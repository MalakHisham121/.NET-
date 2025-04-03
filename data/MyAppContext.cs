using Microsoft.EntityFrameworkCore;
using MVC1.Models;

namespace MVC1.data;

public class MyAppContext : DbContext
{
    public MyAppContext(DbContextOptions<MyAppContext> options):base(options) 
    { }
    public DbSet<Product> p { get; set; }
    public DbSet<Category> categories { get; set; }
    public DbSet<Customer> customers { get; set; }
    public DbSet<Order> crders { get; set; }
    public DbSet<OrderItem> crderItems { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Order>(entity =>
        {
            entity.Property(e => e.TotalAmount)
                .HasColumnType("decimal(18,2)");
        });
    }
}     