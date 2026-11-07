using BookGarden.Data;
using BookGarden.Models;
using BookGarden.Services;
using Microsoft.EntityFrameworkCore;

namespace BookGardenDataAccess.Services;

public sealed class ProductService : IProductService
{
    private readonly ApplicationDbContext _context;

    public ProductService(ApplicationDbContext context) => _context = context;

    public async Task<IReadOnlyList<Product>> GetAllAsync(CancellationToken cancellationToken = default) =>
        await _context.Products
            .AsNoTracking()
            .Include(product => product.Category)
            .OrderBy(product => product.Title)
            .ToListAsync(cancellationToken);

    public Task<Product?> GetByIdAsync(int id, CancellationToken cancellationToken = default) =>
        _context.Products
            .Include(product => product.Category)
            .FirstOrDefaultAsync(product => product.ProductIdentification == id, cancellationToken);

    public async Task<Product> CreateAsync(Product product, CancellationToken cancellationToken = default)
    {
        _context.Products.Add(product);
        await _context.SaveChangesAsync(cancellationToken);
        return product;
    }

    public async Task<bool> UpdateAsync(Product product, CancellationToken cancellationToken = default)
    {
        _context.Products.Update(product);
        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var product = await _context.Products.FindAsync([id], cancellationToken);
        if (product is null) return false;

        _context.Products.Remove(product);
        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }
}
