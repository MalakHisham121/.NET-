using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVC1.data;
using MVC1.Models;

namespace MVC1.Controllers;

public class ProductController:Controller
{
    private readonly MyAppContext _context;

    public ProductController(MyAppContext c)
    {
        _context = c;
        
    }
    public IActionResult Index()
    {
        var p = _context.p.Include(o=>o.Category).ToList();
        ViewData["Products"] = p;
        return View();
    }
    public async Task<IActionResult> getall()
    {
        List<Models.Product> products = new List<Models.Product>
        {
            new Models.Product { ID=1, name = "Laptop", price = 1200.00m, description = "High-performance laptop", imageName = "prod.jpg" },
            new Models.Product {ID=2, name = "Smartphone", price = 800.00m, description = "Latest model smartphone", imageName = "prod.jpg" },
            new Models.Product { ID=3,name = "Tablet", price = 600.00m, description = "Portable tablet device", imageName ="prod.jpg" },
            new Models.Product {ID=4, name = "Headphones", price = 150.00m, description = "Noise-cancelling headphones", imageName = "prod.jpg" },
            new Models.Product {ID=5, name = "Smartwatch", price = 250.00m, description = "Fitness tracking smartwatch", imageName = "prod.jpg" },
            new Models.Product {ID=6, name = "Camera", price = 700.00m, description = "DSLR camera", imageName = "prod.jpg" },
            new Models.Product {ID=7, name = "Printer", price = 300.00m, description = "Wireless printer", imageName = "prod.jpg" },
            new Models.Product {ID=8, name = "Monitor", price = 400.00m, description = "27-inch 4K monitor", imageName = "prod.jpg" },
            new Models.Product {ID=9, name = "Keyboard", price = 100.00m, description = "Mechanical keyboard", imageName = "prod.jpg" },
            new Models.Product {ID=10, name = "Mouse", price = 50.00m, description = "Wireless mouse", imageName ="prod.jpg" }
        };
        
          await  _context.p.AddRangeAsync(products);
        
        await _context.SaveChangesAsync();

        return RedirectToAction("Index"); 
    }
    
    

    public  IActionResult details(int? id)
    {
        if (id == null) return NotFound();
        var p =  _context.p.FirstOrDefault(o => o.ID == id);
        if (p == null) return NotFound();
        ViewBag.Product = p;
        return View();
    }

    [HttpPost]
    public IActionResult create(Product p)
    {
        if (ModelState.IsValid)
        {
            _context.AddRangeAsync(p);
            _context.SaveChanges();
            RedirectToAction(nameof(System.Index));
        }

        ViewData["Categories"] = _context.categories.ToList();
        return View(p);

    }
    
    [HttpPost]
    public IActionResult edit(int? id, Product p)
    {
        if (id == null) return NotFound();
        if (id != p.ID) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(p);
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                
                throw;
            }
            return RedirectToAction(nameof(Index));
        }
        ViewData["Categories"] = _context.categories;
        return View(p);
    }
    public IActionResult delete(int id)
    {
        var product = _context.p.Find(id);
        _context.p.Remove(product);
        _context.SaveChanges();
        return RedirectToAction(nameof(Index));
    }

}