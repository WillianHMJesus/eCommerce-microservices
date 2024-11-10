using EM.Catalog.Domain.Entities;

namespace EM.Catalog.Domain.Interfaces;

public interface IReadRepository
{
    Task AddProductAsync(Product product, CancellationToken cancellationToken);
    Task DeleteProductAsync(Guid productId, CancellationToken cancellationToken);
    Task UpdateProductAsync(Product product, CancellationToken cancellationToken);
    Task<IEnumerable<Product>> GetAllProductsAsync(short page, short pageSize, CancellationToken cancellationToken);
    Task<Product?> GetProductByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<IEnumerable<Product>> GetProductsByCategoryIdAsync(Guid categoryId, short page, short pageSize, CancellationToken cancellationToken);
    Task<IEnumerable<Product>> SearchProductsAsync(string text, short page, short pageSize, CancellationToken cancellationToken);

    Task AddCategoryAsync(Category category, CancellationToken cancellationToken);
    Task DeleteCategoryAsync(Guid categoryId, CancellationToken cancellationToken);
    Task UpdateCategoryAsync(Category category, CancellationToken cancellationToken);
    Task<IEnumerable<Category>> GetAllCategoriesAsync(short page, short pageSize, CancellationToken cancellationToken);
    Task<Category?> GetCategoryByIdAsync(Guid id, CancellationToken cancellationToken);
}
