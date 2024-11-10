using EM.Catalog.Domain.Entities;
using EM.Catalog.Domain.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;

namespace EM.Catalog.Infraestructure.Persistense.Read;

public sealed class ReadRepository : IReadRepository
{
    private readonly CatalogContext _context;

    public ReadRepository(CatalogContext context)
    {
        _context = context;
    }

    public async Task AddProductAsync(Product product, CancellationToken cancellationToken)
    {
        await _context
            .Products
            .InsertOneAsync(product, cancellationToken: cancellationToken);
    }

    public async Task DeleteProductAsync(Guid productId, CancellationToken cancellationToken)
    {
        await _context
            .Products
            .DeleteOneAsync(x => x.Id == productId, cancellationToken: cancellationToken);
    }

    public async Task UpdateProductAsync(Product product, CancellationToken cancellationToken)
    {
        await _context.Products.ReplaceOneAsync(x =>
            x.Id == product.Id,
            product,
            cancellationToken: cancellationToken);
    }

    public async Task<IEnumerable<Product>> GetAllProductsAsync(short page, short pageSize, CancellationToken cancellationToken)
    {
        return await _context.Products.Find(_ => true)
            .Skip((page - 1) * pageSize)
            .Limit(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task<Product?> GetProductByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context
            .Products
            .Find(x => x.Id == id)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IEnumerable<Product>> GetProductsByCategoryIdAsync(Guid categoryId, short page, short pageSize, CancellationToken cancellationToken)
    {
        return await _context
            .Products
            .Find(x => x.CategoryId == categoryId)
            .Skip((page - 1) * pageSize)
            .Limit(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Product>> SearchProductsAsync(string text, short page, short pageSize, CancellationToken cancellationToken)
    {
        var filter = Builders<Product>.Filter.Regex("Name", new BsonRegularExpression(text));

        return await _context
            .Products
            .Find(filter)
            .Skip((page - 1) * pageSize)
            .Limit(pageSize)
            .ToListAsync(cancellationToken);
    }


    public async Task AddCategoryAsync(Category category, CancellationToken cancellationToken)
    {
        await _context
            .Categories
            .InsertOneAsync(category, cancellationToken: cancellationToken);
    }

    public async Task DeleteCategoryAsync(Guid categoryId, CancellationToken cancellationToken)
    {
        await _context
            .Categories
            .DeleteOneAsync(x => x.Id == categoryId, cancellationToken: cancellationToken);
    }

    public async Task UpdateCategoryAsync(Category category, CancellationToken cancellationToken)
    {
        await _context.Categories.ReplaceOneAsync(x =>
            x.Id == category.Id,
            category,
            cancellationToken: cancellationToken);
    }

    public async Task<IEnumerable<Category>> GetAllCategoriesAsync(short page, short pageSize, CancellationToken cancellationToken)
    {
        return await _context.Categories.Find(_ => true)
            .Skip((page - 1) * pageSize)
            .Limit(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task<Category?> GetCategoryByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context
            .Categories
            .Find(x => x.Id == id)
            .FirstOrDefaultAsync(cancellationToken);
    }
}
