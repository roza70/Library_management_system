using BookGarden.Data;
using BookGarden.Models;
using BookGarden.Services;
using Microsoft.EntityFrameworkCore;

namespace BookGardenDataAccess.Services;

public sealed class CategoryService : ICategoryService
{
    private readonly ApplicationDbContext _context;

    public CategoryService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<Category>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return await _context.Categories
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public Task<Category?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        return _context.Categories
            .AsNoTracking()
            .FirstOrDefaultAsync(category => category.CategoryIdentification == id, cancellationToken);
    }

    public Task<bool> NameExistsAsync(
        string name,
        int? excludedId = null,
        CancellationToken cancellationToken = default)
    {
        var normalizedName = name.ToLower();

        return _context.Categories
            .AnyAsync(
                category => category.Name.ToLower() == normalizedName
                    && (!excludedId.HasValue || category.CategoryIdentification != excludedId.Value),
                cancellationToken);
    }

    public async Task<Category> CreateAsync(
        Category category,
        CancellationToken cancellationToken = default)
    {
        _context.Categories.Add(category);
        await _context.SaveChangesAsync(cancellationToken);
        return category;
    }

    public async Task<bool> UpdateAsync(
        Category category,
        CancellationToken cancellationToken = default)
    {
        _context.Categories.Update(category);
        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<bool> DeleteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var category = await _context.Categories
            .FirstOrDefaultAsync(item => item.CategoryIdentification == id, cancellationToken);

        if (category is null)
        {
            return false;
        }

        _context.Categories.Remove(category);
        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }
}
