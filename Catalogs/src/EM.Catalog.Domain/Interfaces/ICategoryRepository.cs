﻿using EM.Catalog.Domain.DTOs;
using EM.Catalog.Domain.Entities;

namespace EM.Catalog.Domain.Interfaces;

public interface ICategoryRepository
{
    #region WriteDatabase
    Task AddCategoryAsync(Category category);
    Task<Category?> GetCategoryByIdAsync(Guid id);
    Task UpdateCategoryAsync(Category category);
    #endregion

    #region ReadDatabase
    Task AddCategoryReadDatabaseAsync(CategoryDTO category);
    Task<IEnumerable<CategoryDTO>> GetAllCategoriesAsync();
    Task UpdateCategoryReadDatabaseAsync(CategoryDTO category);
    #endregion
}