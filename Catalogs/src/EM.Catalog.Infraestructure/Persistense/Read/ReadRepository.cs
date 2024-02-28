using EM.Catalog.Application.Categories.Models;
using EM.Catalog.Application.Interfaces;
using EM.Catalog.Application.Products.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace EM.Catalog.Infraestructure.Persistense.Read;

public sealed class ReadRepository : IReadRepository
{
    private readonly IMongoCollection<ProductDTO> _productsCollection;
    private readonly IMongoCollection<CategoryDTO> _categoriesCollection;

    public ReadRepository(IOptions<CatalogDatabaseSettings> catalogDatabaseSettings)
    {
        MongoClient client = new(catalogDatabaseSettings.Value.ConnectionString);
        IMongoDatabase database = client.GetDatabase(catalogDatabaseSettings.Value.DatabaseName);

        _productsCollection = database.GetCollection<ProductDTO>(catalogDatabaseSettings.Value.ProductsCollectionName);
        _categoriesCollection = database.GetCollection<CategoryDTO>(catalogDatabaseSettings.Value.CategoriesCollectionName);
    }

    public async Task AddProductAsync(ProductDTO product, CancellationToken cancellationToken)
    {
        await _productsCollection.InsertOneAsync(product, cancellationToken: cancellationToken);
    }

    public async Task<IEnumerable<ProductDTO>> GetAllProductsAsync(short page, short pageSize, CancellationToken cancellationToken)
    {
        return await _productsCollection.Find(_ => true)
            .Skip((page - 1) * pageSize)
            .Limit(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task<ProductDTO?> GetProductByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _productsCollection.Find(x => x.Id == id)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IEnumerable<ProductDTO>> GetProductsByCategoryIdAsync(Guid categoryId, CancellationToken cancellationToken)
    {
        return await _productsCollection.Find(x => x.Category.Id == categoryId)
            .ToListAsync(cancellationToken);
    }

    public async Task UpdateProductAsync(ProductDTO product, CancellationToken cancellationToken)
    {
        await _productsCollection.ReplaceOneAsync(x =>
            x.Id == product.Id,
            product,
            cancellationToken: cancellationToken);
    }


    public async Task AddCategoryAsync(CategoryDTO category, CancellationToken cancellationToken)
    {
        await _categoriesCollection.InsertOneAsync(category, cancellationToken: cancellationToken);
    }

    public async Task<IEnumerable<CategoryDTO>> GetAllCategoriesAsync(short page, short pageSize, CancellationToken cancellationToken)
    {
        return await _categoriesCollection.Find(_ => true)
            .Skip((page - 1) * pageSize)
            .Limit(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task<CategoryDTO?> GetCategoryByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _categoriesCollection.Find(x => x.Id == id)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task UpdateCategoryAsync(CategoryDTO category, CancellationToken cancellationToken)
    {
        await _categoriesCollection.ReplaceOneAsync(x =>
            x.Id == category.Id,
            category,
            cancellationToken: cancellationToken);
    }
}
