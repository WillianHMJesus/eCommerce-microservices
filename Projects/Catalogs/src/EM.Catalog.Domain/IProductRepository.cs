using EM.Catalog.Domain.Entities;
using WH.SharedKernel;

namespace EM.Catalog.Domain;

public interface IProductRepository : IRepository<Product>
{
    Task AddAsync(Product product, CancellationToken cancellationToken);
    void Delete(Product product);
    void Update(Product product);
    void UpdateAvailability(Product product);
    Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<IEnumerable<Product>> GetByNameAsync(string name, CancellationToken cancellationToken);

    Task AddCategoryAsync(Category category, CancellationToken cancellationToken);
    void DeleteCategory(Category category);
    void UpdateCategory(Category category);
    Task<IEnumerable<Category>> GetCategoriesByCodeOrName(short code, string name, CancellationToken cancellationToken);
    Task<Category?> GetCategoryByIdAsync(Guid id, CancellationToken cancellationToken);
}
