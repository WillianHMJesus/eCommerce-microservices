using EM.Catalog.Domain.Entities;

namespace EM.Catalog.Domain;

public interface IProductRepository : IRepository<Product>
{
    Task<Product> AddAsync(Product product);
    Task<Category> AddCategoryAsync(Category category);
    Task<Product?> GetByIdAsync(Guid id);
    Task<Category?> GetCategoryByIdAsync(Guid id);
    Task<Product> UpdateAsync(Product product);
    Task<Category> UpdateCategoryAsync(Category category);
}
