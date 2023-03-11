using EM.Catalog.Application.Products.Events.ProductAdded;
using EM.Catalog.Application.Products.Events.ProductUpdated;
using EM.Catalog.Domain.DTOs;
using EM.Catalog.Domain.Entities;
using EM.Catalog.Domain.Interfaces;
using EM.Catalog.Infraestructure.Persistense.Read;
using EM.Catalog.Infraestructure.Persistense.Write;
using MediatR;
using MongoDB.Driver;

namespace EM.Catalog.Infraestructure.Persistense.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly WriteContext _writeContext;
    private readonly ReadContext _readContext;
    private readonly IMediator _mediator;

    public ProductRepository(
        WriteContext writeContext,
        ReadContext readContext,
        IMediator mediator)
    {
        _writeContext = writeContext;
        _readContext = readContext;
        _mediator = mediator;
    }

    #region WriteDatabase
    public async Task AddProductAsync(Product product)
    {
        await _writeContext.Products.AddAsync(product);

        if (await _writeContext.SaveChangesAsync() > 0)
        {
            await _mediator.Publish((ProductAddedEvent)product);
        }
    }

    public async Task<Product?> GetProductByIdAsync(Guid id)
    {
        return await _writeContext.Products.FindAsync(id);
    }

    public async Task UpdateProductAsync(Product product)
    {
        _writeContext.Products.Update(product);

        if (await _writeContext.SaveChangesAsync() > 0)
        {
            await _mediator.Publish((ProductUpdatedEvent)product);
        }
    }
    #endregion

    #region ReadDatabase
    public async Task AddProductAsync(ProductDTO product)
    {
        await _readContext.Products.InsertOneAsync(product);
    }

    public async Task<IEnumerable<ProductDTO>> GetAllProductsAsync()
    {
        return await _readContext.Products.Find(_ => true).ToListAsync();
    }

    public async Task<IEnumerable<ProductDTO>> GetProductsByCategoryIdAsync(Guid categoryId)
    {
        return await _readContext.Products.Find(x => x.Category.Id == categoryId).ToListAsync();
    }

    public async Task UpdateProductAsync(ProductDTO product)
    {
        await _readContext.Products.ReplaceOneAsync(x => x.Id == product.Id, product);
    }
    #endregion
}
