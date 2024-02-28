using EM.Catalog.Domain.Entities;

namespace EM.Catalog.Domain.Interfaces;

public interface IWriteRepository
{
    Task AddProductAsync(Product product, CancellationToken cancellationToken);
    Task UpdateProductAsync(Product product, CancellationToken cancellationToken);

    Task AddCategoryAsync(Category category, CancellationToken cancellationToken);
    Task<Category?> GetCategoryByIdAsync(Guid id, CancellationToken cancellationToken);
    Task UpdateCategoryAsync(Category category, CancellationToken cancellationToken);
}
