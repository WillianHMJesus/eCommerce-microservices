using EM.Catalog.Domain.Entities;
using EM.Catalog.Domain.Interfaces;
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
        await _context.Products.InsertOneAsync(product, cancellationToken: cancellationToken);
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
        return await _context.Products.Find(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IEnumerable<Product>> GetProductsByCategoryIdAsync(Guid categoryId, CancellationToken cancellationToken)
    {
        return await _context.Products.Find(x => x.CategoryId == categoryId).ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Product>> GetProductsByNameAsync(string name, CancellationToken cancellationToken)
    {
        return await _context.Products.Find(x => x.Name == name).ToListAsync(cancellationToken);
    }

    public async Task UpdateProductAsync(Product product, CancellationToken cancellationToken)
    {
        await _context.Products.ReplaceOneAsync(x =>
            x.Id == product.Id,
            product,
            cancellationToken: cancellationToken);
    }


    public async Task AddCategoryAsync(Category category, CancellationToken cancellationToken)
    {
        await _context.Categories.InsertOneAsync(category, cancellationToken: cancellationToken);
    }

    public async Task<IEnumerable<Category>> GetAllCategoriesAsync(short page, short pageSize, CancellationToken cancellationToken)
    {
        return await _context.Categories.Find(_ => true)
            .Skip((page - 1) * pageSize)
            .Limit(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Category>> GetCategoriesByCodeOrName(short code, string name, CancellationToken cancellationToken)
    {
        FilterDefinitionBuilder<Category> builder = Builders<Category>.Filter;
        FilterDefinition<Category> filter = builder.Eq(x => x.Code, code) | builder.Eq(x => x.Name, name);

        return await _context.Categories.Find(filter).ToListAsync(cancellationToken);
    }

    public async Task<Category?> GetCategoryByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.Categories.Find(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task UpdateCategoryAsync(Category category, CancellationToken cancellationToken)
    {
        await _context.Categories.ReplaceOneAsync(x =>
            x.Id == category.Id,
            category,
            cancellationToken: cancellationToken);
    }
}
