using EM.Catalog.Domain;
using EM.Catalog.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EM.Catalog.Infraestructure.Persistense.Write;

public sealed class ProductRepository(CatalogContext context) : IProductRepository
{
    public async Task AddAsync(Product product, CancellationToken cancellationToken)
    {
        await context.Products.AddAsync(product, cancellationToken);
        context.Entry(product).Reference(x => x.Category).Load();
    }

    public void Delete(Product product)
    {
        context.Products.Remove(product);
    }

    public void Update(Product product)
    {
        context.ChangeTracker.Clear();
        context.Products.Attach(product);
        context.Entry(product).State = EntityState.Modified;
        context.Entry(product).Property(x => x.Available).IsModified = false;
        context.Entry(product).Reference(x => x.Category).Load();
    }

    public void UpdateAvailability(Product product)
    {
        context.Products.Attach(product);
        context.Entry(product).Property(x => x.Available).IsModified = true;
        context.Entry(product).Reference(x => x.Category).Load();
    }

    public async Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await context
            .Products
            .FindAsync(id, cancellationToken);
    }

    public async Task<IEnumerable<Product>> GetByNameAsync(string name, CancellationToken cancellationToken)
    {
        return await context
            .Products
            .Where(x => x.Name == name)
            .ToListAsync(cancellationToken);
    }


    public async Task AddCategoryAsync(Category category, CancellationToken cancellationToken)
    {
        await context
            .Categories
            .AddAsync(category, cancellationToken);
    }

    public void DeleteCategory(Category category)
    {
        context.Categories.Remove(category);
    }

    public void UpdateCategory(Category category)
    {
        context.ChangeTracker.Clear();
        context.Categories.Update(category);
    }

    public async Task<IEnumerable<Category>> GetCategoriesByCodeOrName(short code, string name, CancellationToken cancellationToken)
    {
        return await context
            .Categories
            .Where(x => x.Code == code || x.Name == name)
            .ToListAsync(cancellationToken);
    }

    public async Task<Category?> GetCategoryByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await context
            .Categories
            .FindAsync(id, cancellationToken);
    }
}
