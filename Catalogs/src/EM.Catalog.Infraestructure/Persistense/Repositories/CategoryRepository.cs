using EM.Catalog.Application.Categories.Events.CategoryAdded;
using EM.Catalog.Application.Categories.Events.CategoryUpdated;
using EM.Catalog.Domain.DTOs;
using EM.Catalog.Domain.Entities;
using EM.Catalog.Domain.Interfaces;
using EM.Catalog.Infraestructure.Persistense.Read;
using EM.Catalog.Infraestructure.Persistense.Write;
using MediatR;
using MongoDB.Driver;

namespace EM.Catalog.Infraestructure.Persistense.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly WriteContext _writeContext;
    private readonly ReadContext _readContext;
    private readonly IMediator _mediator;

    public CategoryRepository(
        WriteContext writeContext,
        ReadContext readContext,
        IMediator mediator)
    {
        _writeContext = writeContext;
        _readContext = readContext;
        _mediator = mediator;
    }

    #region WriteDatabase
    public async Task AddCategoryAsync(Category category)
    {
        await _writeContext.Categories.AddAsync(category);

        if (await _writeContext.SaveChangesAsync() > 0)
        {
            await _mediator.Publish((CategoryAddedEvent)category);
        }
    }

    public async Task<Category?> GetCategoryByIdAsync(Guid id)
    {
        return await _writeContext.Categories.FindAsync(id);
    }

    public async Task UpdateCategoryAsync(Category category)
    {
        _writeContext.Categories.Update(category);

        if (await _writeContext.SaveChangesAsync() > 0)
        {
            await _mediator.Publish((CategoryUpdatedEvent)category);
        }
    }
    #endregion

    #region ReadDatabase
    public async Task AddCategoryAsync(CategoryDTO category)
    {
        await _readContext.Categories.InsertOneAsync(category);
    }

    public async Task<IEnumerable<CategoryDTO>> GetAllCategoriesAsync(short page = 1, short pageSize = 10)
    {
        return await _readContext.Categories.Find(_ => true)
            .Skip((page - 1) * pageSize)
            .Limit(pageSize)
            .ToListAsync();
    }

    public async Task UpdateCategoryAsync(CategoryDTO category)
    {
        await _readContext.Categories.ReplaceOneAsync(x => x.Id == category.Id, category);
    }
    #endregion
}
