using EM.Catalog.Domain.Entities;

namespace EM.Catalog.Domain;

public interface IProductRepository : IRepository<Product>
{
    Task<Product> AddAsync(Product product);
    Task<Product> UpdateAsync(Product product);
    Task<Product> GetByIdAsync(Guid id);

    Task<Category> AddCategoryAsync(Category category);
    Task<Category> UpdateCategoryAsync(Category category);
    Task<Category> GetCategoryByIdAsync(Guid id);
}
