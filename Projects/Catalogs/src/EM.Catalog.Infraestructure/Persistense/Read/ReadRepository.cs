using EM.Catalog.Domain.Entities;
using EM.Catalog.Domain.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace EM.Catalog.Infraestructure.Persistense.Read;

public sealed class ReadRepository : IReadRepository
{
    private readonly IMongoCollection<Product> _productsCollection;
    private readonly IMongoCollection<Category> _categoriesCollection;

    public ReadRepository(IOptions<CatalogDatabaseSettings> catalogDatabaseSettings)
    {
        MongoClient client = new(catalogDatabaseSettings.Value.ConnectionString);
        IMongoDatabase database = client.GetDatabase(catalogDatabaseSettings.Value.DatabaseName);

        _productsCollection = database.GetCollection<Product>(catalogDatabaseSettings.Value.ProductsCollectionName);
        _categoriesCollection = database.GetCollection<Category>(catalogDatabaseSettings.Value.CategoriesCollectionName);
    }

    public async Task AddProductAsync(Product product, CancellationToken cancellationToken)
    {
        await _productsCollection.InsertOneAsync(product, cancellationToken: cancellationToken);
    }

    public async Task<IEnumerable<Product>> GetAllProductsAsync(short page, short pageSize, CancellationToken cancellationToken)
    {
        return await _productsCollection.Find(_ => true)
            .Skip((page - 1) * pageSize)
            .Limit(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task<Product?> GetProductByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _productsCollection.Find(x => x.Id == id)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IEnumerable<Product>> GetProductsByCategoryIdAsync(Guid categoryId, CancellationToken cancellationToken)
    {
        return await _productsCollection.Find(x => x.CategoryId == categoryId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Product>> GetProductsByCategoryNameAsync(string name, CancellationToken cancellationToken)
    {
        return await _productsCollection.Find(x => x.Name == name)
            .ToListAsync(cancellationToken);
    }

    public async Task UpdateProductAsync(Product product, CancellationToken cancellationToken)
    {
        await _productsCollection.ReplaceOneAsync(x =>
            x.Id == product.Id,
            product,
            cancellationToken: cancellationToken);
    }


    public async Task AddCategoryAsync(Category category, CancellationToken cancellationToken)
    {
        await _categoriesCollection.InsertOneAsync(category, cancellationToken: cancellationToken);
    }

    public async Task<IEnumerable<Category>> GetAllCategoriesAsync(short page, short pageSize, CancellationToken cancellationToken)
    {
        return await _categoriesCollection.Find(_ => true)
            .Skip((page - 1) * pageSize)
            .Limit(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Category>> GetCategoriesByCodeOrName(short code, string name, CancellationToken cancellationToken)
    {
        FilterDefinitionBuilder<Category> builder = Builders<Category>.Filter;
        FilterDefinition<Category> filter = builder.Eq(x => x.Code, code) | builder.Eq(x => x.Name, name);

        return await _categoriesCollection.Find(filter)
            .ToListAsync(cancellationToken);
    }

    public async Task<Category?> GetCategoryByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _categoriesCollection.Find(x => x.Id == id)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task UpdateCategoryAsync(Category category, CancellationToken cancellationToken)
    {
        await _categoriesCollection.ReplaceOneAsync(x =>
            x.Id == category.Id,
            category,
            cancellationToken: cancellationToken);
    }
}
