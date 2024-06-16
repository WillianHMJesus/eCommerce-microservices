using EM.Catalog.Domain.Entities;
using EM.Catalog.Domain.Interfaces;

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

    public void UpdateProduct(Product product)
    {
        _context.Products.Update(product);
        _context.Entry(product).Reference(x => x.Category).Load();
    }

    public async Task AddCategoryAsync(Category category, CancellationToken cancellationToken)
    {
        await _context.Categories.AddAsync(category, cancellationToken);
    }

    public void UpdateCategory(Category category)
    {
        _context.Categories.Update(category);
    }
}
