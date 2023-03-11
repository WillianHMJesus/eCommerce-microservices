using EM.Catalog.Domain.DTOs;
using EM.Catalog.Domain.Entities;
using EM.Catalog.Domain.Interfaces;
using EM.Catalog.Infraestructure.Persistense.Read;
using EM.Catalog.Infraestructure.Persistense.Write;
using MongoDB.Driver;

namespace EM.Catalog.Infraestructure.Persistense.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly WriteContext _writeContext;
    private readonly ReadContext _readContext;

    public CategoryRepository(
        WriteContext writeContext,
        ReadContext readContext)
    {
        _writeContext = writeContext;
        _readContext = readContext;
    }

    #region WriteDatabase
    public async Task AddCategoryAsync(Category category)
    {
        await _writeContext.Categories.AddAsync(category);
        await _writeContext.SaveChangesAsync();
    }

    public async Task<Category?> GetCategoryByIdAsync(Guid id)
    {
        return await _writeContext.Categories.FindAsync(id);
    }

    public async Task UpdateCategoryAsync(Category category)
    {
        _writeContext.Categories.Update(category);
        await _writeContext.SaveChangesAsync();
    }
    #endregion

    #region ReadDatabase
    public async Task AddCategoryAsync(CategoryDTO category)
    {
        await _readContext.Categories.InsertOneAsync(category);
    }

    public async Task<IEnumerable<CategoryDTO>> GetAllCategoriesAsync()
    {
        return await _readContext.Categories.Find(_ => true).ToListAsync();
    }

    public async Task UpdateCategoryAsync(CategoryDTO category)
    {
        await _readContext.Categories.ReplaceOneAsync(x => x.Id == category.Id, category);
    }
    #endregion
}
