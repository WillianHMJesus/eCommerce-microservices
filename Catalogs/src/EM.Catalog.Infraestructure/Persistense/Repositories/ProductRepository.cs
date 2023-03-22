using EM.Catalog.Domain.DTOs;
using EM.Catalog.Domain.Entities;
using EM.Catalog.Domain.Interfaces;
using EM.Catalog.Infraestructure.Persistense.Read;
using EM.Catalog.Infraestructure.Persistense.Write;
using MongoDB.Driver;

namespace EM.Catalog.Infraestructure.Persistense.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly WriteContext _writeContext;
    private readonly ReadContext _readContext;

    public ProductRepository(
        WriteContext writeContext,
        ReadContext readContext)
    {
        _writeContext = writeContext;
        _readContext = readContext;
    }

    public async Task AddProductAsync(Product product)
    {
        await _writeContext.Products.AddAsync(product);
    }

    public async Task<Product?> GetProductByIdAsync(Guid id)
    {
        return await _writeContext.Products.FindAsync(id);
    }

    public async Task UpdateProductAsync(Product product)
    {
        _writeContext.Products.Update(product);

        await Task.CompletedTask;
    }

    public async Task<IEnumerable<ProductDTO>> GetAllProductsAsync(short page = 1, short pageSize = 10)
    {
        return await _readContext.Products.Find(_ => true)
            .Skip((page - 1) * pageSize)
            .Limit(pageSize)
            .ToListAsync();
    }

    public async Task<IEnumerable<ProductDTO>> GetProductsByCategoryIdAsync(Guid categoryId)
    {
        return await _readContext.Products.Find(x => x.Category.Id == categoryId).ToListAsync();
    }
}
