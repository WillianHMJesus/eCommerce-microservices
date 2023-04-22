using EM.Catalog.Domain.Entities;
using EM.Catalog.Domain.Interfaces;
using EM.Catalog.Infraestructure.Persistense.Write;

namespace EM.Catalog.Infraestructure.Persistense.Repositories;

public sealed class ProductRepository : IProductRepository
{
    private readonly WriteContext _writeContext;

    public ProductRepository(WriteContext writeContext)
        => _writeContext = writeContext;

    public async Task AddProductAsync(Product product, CancellationToken cancellationToken)
    {
        await _writeContext.Products.AddAsync(product, cancellationToken);
    }

    public async Task UpdateProductAsync(Product product, CancellationToken cancellationToken)
    {
        Task task = Task.Run(() =>
        {
            _writeContext.Products.Update(product);
        }, cancellationToken);

        await task;
    }

    public async Task AddCategoryAsync(Category category, CancellationToken cancellationToken)
    {
        await _writeContext.Categories.AddAsync(category, cancellationToken);
    }

    public async Task<Category?> GetCategoryByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _writeContext.Categories.FindAsync(id, cancellationToken);
    }

    public async Task UpdateCategoryAsync(Category category, CancellationToken cancellationToken)
    {
        Task task = Task.Run(() =>
        {
            _writeContext.Categories.Update(category);
        }, cancellationToken);
        
        await task;
    }
}
