using EM.Catalog.Domain.Entities;

namespace EM.Catalog.Domain.Interfaces;

public interface IWriteRepository
{
    Task AddProductAsync(Product product, CancellationToken cancellationToken);
    void UpdateProduct(Product product);

    Task AddCategoryAsync(Category category, CancellationToken cancellationToken);
    void UpdateCategory(Category category);
}
