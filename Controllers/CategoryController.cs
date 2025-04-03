using Microsoft.AspNetCore.Mvc;
using MVC1.Models;
using Microsoft.EntityFrameworkCore;
using MVC1.data;

namespace MVC1.Controllers;

public class CategoryController : Controller
{
    private readonly MyAppContext _context;

    public CategoryController(MyAppContext c)
    {
        _context = c;
    }

    public IActionResult Index()
    {
        var cats = _context.categories.ToList();
        ViewData["Categories"] = cats;
        return View();
    }

    public IActionResult Details(int? id)
    {
        if (id == null) return NotFound();
        var cat = _context.categories.Include(c => c.Products).FirstOrDefault(p => p.ID == id);
        if (cat == null) return NotFound();
        ViewBag.Category = cat;
        return View();
    }

    public IActionResult Create()
    { 
        return View();
    }
    
    [HttpPost]
    public IActionResult Create(Category cat)
    {
        if (ModelState.IsValid)
        {
            _context.Add(cat);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        return View(cat);
    }

    public IActionResult Edit(int? id)
    {
        if (id == null) return NotFound();
        var cat = _context.categories.Find(id);
        if (cat == null) return NotFound();
        return View(cat);
    }

    [HttpPost]
    public IActionResult Edit(int id, Category cat)
    {
        if (id != cat.ID) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(cat);
                _context.SaveChanges();
            }
            catch(DbUpdateConcurrencyException)
            {
                if (!CategoryExists(cat.ID))
                {
                    return NotFound();
                }
                throw;
            }
            return RedirectToAction(nameof(Index));
        }
        return View(cat);
    }

    public IActionResult Delete(int? id)
    {
        if (id == null) return NotFound();
        var cat = _context.categories.FirstOrDefault(p => p.ID == id);
        if (cat == null) return NotFound();
        return View(cat);
    }

    [HttpPost]
    public IActionResult Delete(int id)
    {
        var cat = _context.categories.Find(id);
        _context.categories.Remove(cat);
        _context.SaveChanges();
        return RedirectToAction(nameof(Index));
    }

    private bool CategoryExists(int id)
    {
        return _context.categories.Any(e => e.ID == id);
    }
}