using EM.Catalog.Domain.Entities;
using EM.Catalog.Domain.Interfaces;

namespace EM.Catalog.Infraestructure.Persistense.Write;

public sealed class WriteRepository : IWriteRepository
{
    private readonly CatalogContext _writeContext;

    public WriteRepository(CatalogContext writeContext)
        => _writeContext = writeContext;

    public async Task AddProductAsync(Product product, CancellationToken cancellationToken)
    {
        await _writeContext.Products.AddAsync(product, cancellationToken);
        _writeContext.Entry(product).Reference(x => x.Category).Load();
    }

    public void UpdateProduct(Product product)
    {
        _writeContext.Products.Update(product);
        _writeContext.Entry(product).Reference(x => x.Category).Load();
    }

    public async Task AddCategoryAsync(Category category, CancellationToken cancellationToken)
    {
        await _writeContext.Categories.AddAsync(category, cancellationToken);
    }

    public void UpdateCategory(Category category)
    {
        _writeContext.Categories.Update(category);
    }
}
