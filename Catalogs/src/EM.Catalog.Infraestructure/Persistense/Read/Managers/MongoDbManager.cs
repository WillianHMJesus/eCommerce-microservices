using EM.Catalog.Application.DTOs;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace EM.Catalog.Infraestructure.Persistense.Read.Managers;

public sealed class MongoDbManager : IDatabaseReadManager
{
    private readonly MongoDbConfiguration _dbConfiguration;

    public MongoDbManager(MongoDbConfiguration dbConfiguration)
        => _dbConfiguration = dbConfiguration;

    public async Task AddProductAsync(ProductDTO product, CancellationToken cancellationToken)
    {
        await _dbConfiguration.Products.InsertOneAsync(product, cancellationToken: cancellationToken);
    }

    public async Task<IEnumerable<ProductDTO>> GetAllProductsAsync(short page, short pageSize, CancellationToken cancellationToken)
    {
        return await _dbConfiguration.Products.Find(_ => true)
            .Skip((page - 1) * pageSize)
            .Limit(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task<ProductDTO?> GetProductByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _dbConfiguration.Products.Find(x => x.Id == id)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IEnumerable<ProductDTO>> GetProductsByCategoryIdAsync(Guid categoryId, CancellationToken cancellationToken)
    {
        return await _dbConfiguration.Products.Find(x => x.Category.Id == categoryId)
            .ToListAsync(cancellationToken);
    }

    public async Task UpdateProductAsync(ProductDTO product, CancellationToken cancellationToken)
    {
        await _dbConfiguration.Products.ReplaceOneAsync(x => 
            x.Id == product.Id, 
            product, 
            cancellationToken: cancellationToken);
    }


    public async Task AddCategoryAsync(CategoryDTO category, CancellationToken cancellationToken)
    {
        await _dbConfiguration.Categories.InsertOneAsync(category, cancellationToken: cancellationToken);
    }

    public async Task<IEnumerable<CategoryDTO>> GetAllCategoriesAsync(short page, short pageSize, CancellationToken cancellationToken)
    {
        return await _dbConfiguration.Categories.Find(_ => true)
            .Skip((page - 1) * pageSize)
            .Limit(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task<CategoryDTO?> GetCategoryByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _dbConfiguration.Categories.Find(x => x.Id == id)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task UpdateCategoryAsync(CategoryDTO category, CancellationToken cancellationToken)
    {
        await _dbConfiguration.Categories.ReplaceOneAsync(x => 
            x.Id == category.Id, 
            category, 
            cancellationToken: cancellationToken);
    }
}
