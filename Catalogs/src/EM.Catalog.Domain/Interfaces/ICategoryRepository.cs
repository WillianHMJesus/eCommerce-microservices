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
    Task AddCategoryAsync(CategoryDTO category);
    Task<IEnumerable<CategoryDTO>> GetAllCategoriesAsync(short page = 1, short pageSize = 10);
    Task UpdateCategoryAsync(CategoryDTO category);
    #endregion
}