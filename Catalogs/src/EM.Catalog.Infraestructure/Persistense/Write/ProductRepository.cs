using EM.Catalog.Domain;
using EM.Catalog.Domain.Entities;

namespace EM.Catalog.Infraestructure.Persistense.Write;

public class ProductRepository : IProductRepository
{
    private readonly CatalogContext _context;

    public ProductRepository(CatalogContext context)
    {
        _context = context;
    }

    public async Task<Product> AddAsync(Product product)
    {
        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();

        return product;
    }

    public async Task<Category> AddCategoryAsync(Category category)
    {
        await _context.Categories.AddAsync(category);
        await _context.SaveChangesAsync();

        return category;
    }

    public async Task<Product?> GetByIdAsync(Guid id)
    {
        return await _context.Products.FindAsync(id);
    }

    public async Task<Category?> GetCategoryByIdAsync(Guid id)
    {
        return await _context.Categories.FindAsync(id);
    }

    public async Task<Product> UpdateAsync(Product product)
    {
        _context.Products.Update(product);
        await _context.SaveChangesAsync();

        return product;
    }

    public async Task<Category> UpdateCategoryAsync(Category category)
    {
        _context.Categories.Update(category);
        await _context.SaveChangesAsync();

        return category;
    }
}
