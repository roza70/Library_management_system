
using System;
using System.IO;
using System.Linq;
using BookGarden.Data;
using BookGarden.Models;
using BookGarden.Models.ViewModels;
using BookGarden.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BookGarden.Areas.Admin.Controllers;

[Area("Admin")]
public class ProductController : Controller
{
    private readonly IProductService _productService;
    private readonly ApplicationDbContext _context;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public ProductController(
        IProductService productService,
        ApplicationDbContext context,
        IWebHostEnvironment webHostEnvironment)
    {
        _productService = productService;
        _context = context;
        _webHostEnvironment = webHostEnvironment;
    }

    // INDEX
    public async Task<IActionResult> Index(
        CancellationToken cancellationToken)
    {
        var products = await _productService.GetAllAsync(cancellationToken);
        return View(products);
    }

    // CREATE - GET
    [HttpGet]
    public async Task<IActionResult> Create(
        CancellationToken cancellationToken)
    {
        await LoadCategoriesAsync(cancellationToken);

        var viewModel = new ProductViewModel
        {
            Product = new Product()
        };

        return View(viewModel);
    }

    // CREATE - POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        ProductViewModel viewModel,
        CancellationToken cancellationToken)
    {
        if (ModelState.IsValid)
        {
            try
            {
                var imageUrl = await SaveImageAsync(viewModel.ImageFile);

                if (imageUrl != null)
                {
                    viewModel.Product.ImageUrl = imageUrl;
                }

                await _productService.CreateAsync(
                    viewModel.Product,
                    cancellationToken);

                TempData["success"] = "Product created successfully.";

                return RedirectToAction(nameof(Index));
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(
                    nameof(viewModel.ImageFile),
                    ex.Message);
            }
        }

        await LoadCategoriesAsync(cancellationToken);

        return View(viewModel);
    }

    // UPDATE - GET
    [HttpGet]
    public async Task<IActionResult> Update(
        int? id,
        CancellationToken cancellationToken)
    {
        if (id is null or 0)
            return NotFound();

        var product = await _productService.GetByIdAsync(
            id.Value,
            cancellationToken);

        if (product is null)
            return NotFound();

        await LoadCategoriesAsync(cancellationToken);

        var viewModel = new ProductViewModel
        {
            Product = product
        };

        return View(viewModel);
    }

    // UPDATE - POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(
        ProductViewModel viewModel,
        CancellationToken cancellationToken)
    {
        var existingProduct = await _productService.GetByIdAsync(
            viewModel.Product.ProductIdentification,
            cancellationToken);

        if (existingProduct is null)
            return NotFound();

        // Store the original image URL before updating the tracked entity.
        var oldImageUrl = existingProduct.ImageUrl;

        // Preserve the existing image unless a new one is uploaded.
        viewModel.Product.ImageUrl = oldImageUrl;

        string? newImageUrl = null;

        if (ModelState.IsValid)
        {
            try
            {
                newImageUrl = await SaveImageAsync(viewModel.ImageFile);

                if (newImageUrl != null)
                {
                    viewModel.Product.ImageUrl = newImageUrl;
                }

                var updated = await _productService.UpdateAsync(
                    viewModel.Product,
                    cancellationToken);

                if (updated)
                {
                    // Delete the original image only after successful update.
                    if (newImageUrl != null)
                    {
                        DeleteImage(oldImageUrl);
                    }

                    TempData["success"] = "Product updated successfully.";

                    return RedirectToAction(nameof(Index));
                }

                // Clean up the newly uploaded image if update fails.
                if (newImageUrl != null)
                {
                    DeleteImage(newImageUrl);
                }

                ModelState.AddModelError(
                    string.Empty,
                    "Product could not be updated.");
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(
                    nameof(viewModel.ImageFile),
                    ex.Message);

                if (newImageUrl != null)
                {
                    DeleteImage(newImageUrl);
                }
            }
        }

        await LoadCategoriesAsync(cancellationToken);

        return View(viewModel);
    }

    // DELETE - GET
    [HttpGet]
    public async Task<IActionResult> Delete(
        int? id,
        CancellationToken cancellationToken)
    {
        if (id is null or 0)
            return NotFound();

        var product = await _productService.GetByIdAsync(
            id.Value,
            cancellationToken);

        return product is null ? NotFound() : View(product);
    }

    // DELETE - POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(
        Product product,
        CancellationToken cancellationToken)
    {
        var existingProduct = await _productService.GetByIdAsync(
            product.ProductIdentification,
            cancellationToken);

        if (existingProduct is null)
            return NotFound();

        // Store image URL before deleting the database entity.
        var imageUrl = existingProduct.ImageUrl;

        var deleted = await _productService.DeleteAsync(
            product.ProductIdentification,
            cancellationToken);

        if (!deleted)
            return NotFound();

        // Delete associated image after successful database deletion.
        DeleteImage(imageUrl);

        TempData["success"] = "Product deleted successfully.";

        return RedirectToAction(nameof(Index));
    }

    // LOAD CATEGORY DROPDOWN
    private async Task LoadCategoriesAsync(
        CancellationToken cancellationToken)
    {
        ViewBag.Categories = new SelectList(
            await _context.Categories
                .AsNoTracking()
                .OrderBy(c => c.Name)
                .ToListAsync(cancellationToken),

            nameof(Category.CategoryIdentification),
            nameof(Category.Name));
    }

    // SAVE PRODUCT IMAGE
    private async Task<string?> SaveImageAsync(IFormFile? imageFile)
    {
        if (imageFile == null || imageFile.Length == 0)
            return null;

        // Maximum file size: 5 MB
        if (imageFile.Length > 5 * 1024 * 1024)
        {
            throw new InvalidOperationException(
                "Image size cannot exceed 5 MB.");
        }

        // Allowed image extensions
        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };

        var extension = Path.GetExtension(imageFile.FileName)
            .ToLowerInvariant();

        if (!allowedExtensions.Contains(extension))
        {
            throw new InvalidOperationException(
                "Only JPG, PNG, and WEBP images are allowed.");
        }

        // Generate unique filename
        var fileName = $"{Guid.NewGuid()}{extension}";

        // Create upload folder
        var uploadsFolder = Path.Combine(
            _webHostEnvironment.WebRootPath,
            "images",
            "products");

        Directory.CreateDirectory(uploadsFolder);

        // Full file path
        var filePath = Path.Combine(uploadsFolder, fileName);

        // Save image
        await using (var fileStream = new FileStream(
            filePath,
            FileMode.Create))
        {
            await imageFile.CopyToAsync(fileStream);
        }

        return $"/images/products/{fileName}";
    }

    // DELETE PRODUCT IMAGE FILE
    private void DeleteImage(string? imageUrl)
    {
        if (string.IsNullOrWhiteSpace(imageUrl))
            return;

        const string imageFolder = "/images/products/";

        if (!imageUrl.StartsWith(
                imageFolder,
                StringComparison.OrdinalIgnoreCase))
        {
            return;
        }

        var fileName = Path.GetFileName(imageUrl);

        if (string.IsNullOrWhiteSpace(fileName))
            return;

        var filePath = Path.Combine(
            _webHostEnvironment.WebRootPath,
            "images",
            "products",
            fileName);

        if (System.IO.File.Exists(filePath))
        {
            System.IO.File.Delete(filePath);
        }
    }
}
