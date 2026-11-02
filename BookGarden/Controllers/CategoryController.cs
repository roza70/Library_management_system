using BookGarden.Data;
using BookGarden.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookGarden.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var categories = _context.Categories.ToList();
            return View(categories);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category category)
        {
            if (!string.IsNullOrEmpty(category.Name) &&
                _context.Categories.Any(c => c.Name.ToLower() == category.Name.ToLower()))
            {
                ModelState.AddModelError("", "Category name already exists!");
            }
            if (ModelState.IsValid)
            {

                _context.Categories.Add(category);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(category);
        }

        [HttpGet]
        public IActionResult Update(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var category = _context.Categories.Find(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(Category category)
        {
            if (!string.IsNullOrEmpty(category.Name) &&
                _context.Categories.Any(c => c.CategoryIdentification != category.CategoryIdentification &&
                    c.Name.ToLower() == category.Name.ToLower()))
            {
                ModelState.AddModelError("", "Category name already exists!");
            }
            if (ModelState.IsValid)
            {
                _context.Categories.Update(category);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(category);
        }
    }
}
