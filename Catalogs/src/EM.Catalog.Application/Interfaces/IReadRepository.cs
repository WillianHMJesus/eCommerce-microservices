using EM.Catalog.Application.Categories.Models;
using EM.Catalog.Application.Products.Models;

namespace EM.Catalog.Application.Interfaces;

public interface IReadRepository
{
    Task AddProductAsync(ProductDTO product, CancellationToken cancellationToken);
    Task<IEnumerable<ProductDTO>> GetAllProductsAsync(short page, short pageSize, CancellationToken cancellationToken);
    Task<ProductDTO?> GetProductByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<IEnumerable<ProductDTO>> GetProductsByCategoryIdAsync(Guid categoryId, CancellationToken cancellationToken);
    Task UpdateProductAsync(ProductDTO product, CancellationToken cancellationToken);

    Task AddCategoryAsync(CategoryDTO category, CancellationToken cancellationToken);
    Task<IEnumerable<CategoryDTO>> GetAllCategoriesAsync(short page, short pageSize, CancellationToken cancellationToken);
    Task<CategoryDTO?> GetCategoryByIdAsync(Guid id, CancellationToken cancellationToken);
    Task UpdateCategoryAsync(CategoryDTO category, CancellationToken cancellationToken);
}
