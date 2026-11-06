using BookGarden.Models;
using BookGarden.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookGarden.Areas.Admin.Controllers;

[Area("Admin")]
public class CategoryController : Controller
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var categories = await _categoryService.GetAllAsync(cancellationToken);
        return View(categories);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Category category, CancellationToken cancellationToken)
    {
        if (!string.IsNullOrEmpty(category.Name) &&
            await _categoryService.NameExistsAsync(category.Name, cancellationToken: cancellationToken))
        {
            ModelState.AddModelError("", "Category name already exists!");
        }

        if (ModelState.IsValid)
        {
            await _categoryService.CreateAsync(category, cancellationToken);
            TempData["success"] = "Category created successfully!";
            return RedirectToAction(nameof(Index));
        }

        return View(category);
    }

    [HttpGet]
    public async Task<IActionResult> Update(int? id, CancellationToken cancellationToken)
    {
        if (id is null or 0)
        {
            return NotFound();
        }

        var category = await _categoryService.GetByIdAsync(id.Value, cancellationToken);
        if (category is null)
        {
            return NotFound();
        }

        return View(category);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(Category category, CancellationToken cancellationToken)
    {
        if (!string.IsNullOrEmpty(category.Name) &&
            await _categoryService.NameExistsAsync(
                category.Name,
                category.CategoryIdentification,
                cancellationToken))
        {
            ModelState.AddModelError("", "Category name already exists!");
        }

        if (ModelState.IsValid)
        {
            await _categoryService.UpdateAsync(category, cancellationToken);
            TempData["success"] = "Category updated successfully";
            return RedirectToAction(nameof(Index));
        }

        return View(category);
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int? id, CancellationToken cancellationToken)
    {
        if (id is null or 0)
        {
            return NotFound();
        }

        var category = await _categoryService.GetByIdAsync(id.Value, cancellationToken);
        if (category is null)
        {
            return NotFound();
        }

        return View(category);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(Category category, CancellationToken cancellationToken)
    {
        if (!await _categoryService.DeleteAsync(category.CategoryIdentification, cancellationToken))
        {
            return NotFound();
        }

        TempData["success"] = "Category deleted successfully";
        return RedirectToAction(nameof(Index));
    }
}
