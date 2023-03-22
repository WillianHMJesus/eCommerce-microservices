using EM.Catalog.Domain.DTOs;
using EM.Catalog.Domain.Entities;

namespace EM.Catalog.Domain.Interfaces;

public interface ICategoryRepository
{
    Task AddCategoryAsync(Category category);
    Task<Category?> GetCategoryByIdAsync(Guid id);
    Task UpdateCategoryAsync(Category category);
    Task<IEnumerable<CategoryDTO>> GetAllCategoriesAsync(short page = 1, short pageSize = 10);
}