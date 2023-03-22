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

    public async Task AddCategoryAsync(Category category)
    {
        await _writeContext.Categories.AddAsync(category);
    }

    public async Task<Category?> GetCategoryByIdAsync(Guid id)
    {
        return await _writeContext.Categories.FindAsync(id);
    }

    public async Task UpdateCategoryAsync(Category category)
    {
        _writeContext.Categories.Update(category);

        await Task.CompletedTask;
    }

    public async Task<IEnumerable<CategoryDTO>> GetAllCategoriesAsync(short page = 1, short pageSize = 10)
    {
        return await _readContext.Categories.Find(_ => true)
            .Skip((page - 1) * pageSize)
            .Limit(pageSize)
            .ToListAsync();
    }
}
