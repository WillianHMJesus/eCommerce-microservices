using EM.Catalog.Domain.Entities;
using EM.Catalog.Domain.Interfaces;
using EM.Catalog.Infraestructure.Persistense.Write;

namespace EM.Catalog.Infraestructure.Persistense.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly WriteContext _writeContext;

    public ProductRepository(WriteContext writeContext)
        => _writeContext = writeContext;

    public async Task AddProductAsync(Product product)
    {
        await _writeContext.Products.AddAsync(product);
    }

    public async Task UpdateProductAsync(Product product)
    {
        _writeContext.Products.Update(product);

        await Task.CompletedTask;
    }


    public async Task AddCategoryAsync(Category category)
    {
        await _writeContext.Categories.AddAsync(category);
    }

    public async Task<Category?> GetCategoryByIdAsync(Guid id)
    {
        return await _writeContext.Categories.FindAsync(id);
    }

    public async Task UpdateCategoryAsync(Category category)
    {
        _writeContext.Categories.Update(category);

        await Task.CompletedTask;
    }
}
