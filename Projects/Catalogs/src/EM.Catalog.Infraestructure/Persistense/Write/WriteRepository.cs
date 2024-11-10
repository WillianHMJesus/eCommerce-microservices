using EM.Catalog.Domain.Entities;
using EM.Catalog.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EM.Catalog.Infraestructure.Persistense.Write;

public sealed class WriteRepository : IWriteRepository
{
    private readonly CatalogContext _context;

    public WriteRepository(CatalogContext context)
        => _context = context;

    public async Task AddProductAsync(Product product, CancellationToken cancellationToken)
    {
        await _context.Products.AddAsync(product, cancellationToken);
        _context.Entry(product).Reference(x => x.Category).Load();
    }

    public void DeleteProduct(Product product)
    {
        _context.Products.Attach(product);
        _context.Entry(product).Property(x => x.Active).IsModified = true;
    }

    public void UpdateProduct(Product product)
    {
        Product? entity = _context.Products.Find(product.Id);

        if (!entity?.Available ?? false)
        {
            product.MakeUnavailable();
        }

        _context.ChangeTracker.Clear();
        _context.Update(product);
        _context.Entry(product).Reference(x => x.Category).Load();
    }

    public void UpdateProductAvailable(Product product)
    {
        _context.Products.Attach(product);
        _context.Entry(product).Property(x => x.Available).IsModified = true;
        _context.Entry(product).Reference(x => x.Category).Load();
    }

    public async Task<Product?> GetProductByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context
            .Products
            .FindAsync(id, cancellationToken);
    }

    public async Task<IEnumerable<Product>> GetProductsByNameAsync(string name, CancellationToken cancellationToken)
    {
        return await _context
            .Products
            .Where(x => x.Name == name)
            .ToListAsync(cancellationToken);
    }


    public async Task AddCategoryAsync(Category category, CancellationToken cancellationToken)
    {
        await _context
            .Categories
            .AddAsync(category, cancellationToken);
    }

    public void DeleteCategory(Category category)
    {
        _context.Categories.Attach(category);
        _context.Entry(category).Property(x => x.Active).IsModified = true;
    }

    public void UpdateCategory(Category category)
    {
        _context.ChangeTracker.Clear();
        _context.Categories.Update(category);
    }

    public async Task<IEnumerable<Category>> GetCategoriesByCodeOrName(short code, string name, CancellationToken cancellationToken)
    {
        return await _context
            .Categories
            .Where(x => x.Code == code || x.Name == name)
            .ToListAsync(cancellationToken);
    }

    public async Task<Category?> GetCategoryByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context
            .Categories
            .FindAsync(id, cancellationToken);
    }
}
