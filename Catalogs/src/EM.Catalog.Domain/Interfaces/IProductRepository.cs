using EM.Catalog.Domain.Entities;

namespace EM.Catalog.Domain.Interfaces;

public interface IProductRepository
{
    Task AddProductAsync(Product product);
    Task UpdateProductAsync(Product product);

    Task AddCategoryAsync(Category category);
    Task<Category?> GetCategoryByIdAsync(Guid id);
    Task UpdateCategoryAsync(Category category);
}
