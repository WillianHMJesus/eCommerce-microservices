using EM.Catalog.Application.Categories;
using EM.Catalog.Application.Products;

namespace EM.Catalog.Application.Interfaces;

public interface IProductReadRepository
{
    Task AddAsync(ProductDTO product, CancellationToken cancellationToken);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken);
    Task UpdateAsync(ProductDTO product, CancellationToken cancellationToken);
    Task<IEnumerable<ProductDTO>> GetAllAsync(short page, short pageSize, CancellationToken cancellationToken);
    Task<IEnumerable<ProductDTO>> GetByCategoryIdAsync(Guid categoryId, short page, short pageSize, CancellationToken cancellationToken);
    Task<ProductDTO?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<IEnumerable<ProductDTO>> SearchAsync(string text, short page, short pageSize, CancellationToken cancellationToken);

    Task AddCategoryAsync(CategoryDTO category, CancellationToken cancellationToken);
    Task DeleteCategoryAsync(Guid categoryId, CancellationToken cancellationToken);
    Task UpdateCategoryAsync(CategoryDTO category, CancellationToken cancellationToken);
    Task<IEnumerable<CategoryDTO>> GetAllCategoriesAsync(short page, short pageSize, CancellationToken cancellationToken);
    Task<CategoryDTO?> GetCategoryByIdAsync(Guid categoryId, CancellationToken cancellationToken);
}
