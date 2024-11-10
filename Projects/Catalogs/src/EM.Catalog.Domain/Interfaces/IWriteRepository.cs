using EM.Catalog.Domain.Entities;

namespace EM.Catalog.Domain.Interfaces;

public interface IWriteRepository
{
    Task AddProductAsync(Product product, CancellationToken cancellationToken);
    void DeleteProduct(Product product);
    void UpdateProduct(Product product);
    void UpdateProductAvailable(Product product);
    Task<Product?> GetProductByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<IEnumerable<Product>> GetProductsByNameAsync(string name, CancellationToken cancellationToken);

    Task AddCategoryAsync(Category category, CancellationToken cancellationToken);
    void DeleteCategory(Category category);
    void UpdateCategory(Category category);
    Task<IEnumerable<Category>> GetCategoriesByCodeOrName(short code, string name, CancellationToken cancellationToken);
    Task<Category?> GetCategoryByIdAsync(Guid id, CancellationToken cancellationToken);
}
