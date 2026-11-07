using BookGarden.Models;
using BookGarden.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookGarden.Controllers;

[ApiController]
[Route("api/products")]
public sealed class ProductsApiController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsApiController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ProductResponse>>> GetAll(
        CancellationToken cancellationToken)
    {
        var products = await _productService.GetAllAsync(cancellationToken);
        return Ok(products.Select(ProductResponse.From).ToList());
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProductResponse>> GetById(
        int id,
        CancellationToken cancellationToken)
    {
        var product = await _productService.GetByIdAsync(id, cancellationToken);
        return product is null ? NotFound() : Ok(ProductResponse.From(product));
    }

    public sealed record ProductResponse(
        int Id,
        string Title,
        string Author,
        string Description,
        string Isbn,
        double ListPrice,
        double Price50,
        double Price100,
        int CategoryId,
        string? CategoryName)
    {
        public static ProductResponse From(Product product) => new(
            product.ProductIdentification,
            product.Title,
            product.Author,
            product.Description,
            product.ISBN,
            product.ListPrice,
            product.Price50,
            product.Price100,
            product.CategoryIdentification,
            product.Category?.Name);
    }
}
