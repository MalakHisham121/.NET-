using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVC1.data;
using MVC1.Models;

namespace MVC1.Controllers;

public class OrderController : Controller
{
    private readonly MyAppContext _context;

    public OrderController(MyAppContext context)
    {
        _context = context;
    }

    
    public IActionResult Index()
    {
        var orders = _context.crders
            .Include(o => o.Customer)
            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.product)
            .ToList();
        
        ViewData["Orders"] = orders.ToList();
        return View();
    }
    public IActionResult Details(int? id)
    {
        if (id == null) return NotFound();

        var order = _context.crders
            .Include(o => o.Customer)
            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.product)
            .FirstOrDefault(m => m.Id == id);

        if (order == null) return NotFound();

        ViewBag.Order = order;
        return View();
    }

    
    public IActionResult Create()
    {
        ViewData["Customers"] = _context.customers.ToList();
        ViewData["Products"] = _context.p.ToList();
        return View();
    }

    // POST: Order/Create
    [HttpPost]
    public IActionResult Create(Order order, List<OrderItem> orderItems)
    {
        if (ModelState.IsValid)
        {
            decimal total = 0;
            foreach (var item in orderItems)
            {
                var product = _context.p.Find(item.Productid);
                item.price = product.price;
                total += item.price * item.quantity;
            }

            order.OrderDate = DateTime.Now;
            order.TotalAmount = total;
            order.OrderItems = orderItems;

            _context.Add(order);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        ViewData["Customers"] = _context.customers.ToList();
        ViewData["Products"] = _context.p.ToList();
        return View(order);
    }

    public IActionResult Edit(int? id)
    {
        if (id == null) return NotFound();

        var order = _context.crders
            .Include(o => o.OrderItems)
            .FirstOrDefault(m => m.Id == id);

        if (order == null) return NotFound();

        ViewData["Customers"] = _context.customers.ToList();
        ViewData["Products"] = _context.p.ToList();
        return View(order);
    }

    [HttpPost]
    public IActionResult Edit(int id, Order order, List<OrderItem> orderItems)
    {
        if (id != order.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            { 
                decimal total = 0;
                foreach (var item in orderItems)
                {
                    var product = _context.p.Find(item.Productid);
                    item.price = product.price;
                    total += item.price * item.quantity;
                }

                order.TotalAmount = total;

                var existingItems = _context.crderItems.Where(oi => oi.Orderid == order.Id);
                _context.crderItems.RemoveRange(existingItems);

                foreach (var item in orderItems)
                {
                    item.Orderid = order.Id;
                    _context.Add(item);
                }

                _context.Update(order);
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
            return RedirectToAction(nameof(Index));
        }

        ViewData["Customers"] = _context.customers.ToList();
        ViewData["Products"] = _context.p.ToList();
        return View(order);
    }

    public IActionResult Delete(int? id)
    {
        if (id == null) return NotFound();

        var order = _context.crders
            .Include(o => o.Customer)
            .FirstOrDefault(m => m.Id == id);

        if (order == null) return NotFound();

        return View(order);
    }

    [HttpPost]
    public IActionResult Delete(int id)
    {
        var order = _context.crders.Find(id);
        _context.crders.Remove(order);
        _context.SaveChanges();
        return RedirectToAction(nameof(Index));
    }

   
}