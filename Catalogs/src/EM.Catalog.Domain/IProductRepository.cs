using EM.Catalog.Domain.Entities;

namespace EM.Catalog.Domain;

public interface IProductRepository : IRepository<Product>
{
    Task<Product> Add(Product product);
    Task<Product> Update(Product product);
    Task<Product> Delete(Product product);
    Task<Product> GetById(Guid id);

    Task<Category> AddCategory(Category category);
    Task<Category> UpdateCategory(Category category);
    Task<Category> DeleteCategory(Guid id);
    Task<Category> GetCategoryById(Guid id);
}
