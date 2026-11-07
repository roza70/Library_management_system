using BookGarden.Data;
using BookGarden.Models;
using BookGarden.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BookGarden.Areas.Admin.Controllers;

[Area("Admin")]
public class ProductController : Controller
{
    private readonly IProductService _productService;
    private readonly ApplicationDbContext _context;

    public ProductController(IProductService productService, ApplicationDbContext context)
    {
        _productService = productService;
        _context = context;
    }

    public async Task<IActionResult> Index(CancellationToken cancellationToken) =>
        View(await _productService.GetAllAsync(cancellationToken));

    [HttpGet]
    public async Task<IActionResult> Create(CancellationToken cancellationToken)
    {
        await LoadCategoriesAsync(cancellationToken);
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Product product, CancellationToken cancellationToken)
    {
        if (ModelState.IsValid)
        {
            await _productService.CreateAsync(product, cancellationToken);
            TempData["success"] = "Product created successfully.";
            return RedirectToAction(nameof(Index));
        }

        await LoadCategoriesAsync(cancellationToken);
        return View(product);
    }

    [HttpGet]
    public async Task<IActionResult> Update(int? id, CancellationToken cancellationToken)
    {
        if (id is null or 0) return NotFound();
        var product = await _productService.GetByIdAsync(id.Value, cancellationToken);
        if (product is null) return NotFound();

        await LoadCategoriesAsync(cancellationToken);
        return View(product);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(Product product, CancellationToken cancellationToken)
    {
        if (ModelState.IsValid)
        {
            await _productService.UpdateAsync(product, cancellationToken);
            TempData["success"] = "Product updated successfully.";
            return RedirectToAction(nameof(Index));
        }

        await LoadCategoriesAsync(cancellationToken);
        return View(product);
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int? id, CancellationToken cancellationToken)
    {
        if (id is null or 0) return NotFound();
        var product = await _productService.GetByIdAsync(id.Value, cancellationToken);
        return product is null ? NotFound() : View(product);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(Product product, CancellationToken cancellationToken)
    {
        if (!await _productService.DeleteAsync(product.ProductIdentification, cancellationToken))
            return NotFound();

        TempData["success"] = "Product deleted successfully.";
        return RedirectToAction(nameof(Index));
    }

    private async Task LoadCategoriesAsync(CancellationToken cancellationToken)
    {
        ViewBag.Categories = new SelectList(
            await _context.Categories.AsNoTracking().OrderBy(c => c.Name).ToListAsync(cancellationToken),
            nameof(Category.CategoryIdentification),
            nameof(Category.Name));
    }
}
