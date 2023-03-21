using EM.Catalog.Application.Categories.Events.CategoryAdded;
using EM.Catalog.Application.Categories.Events.CategoryUpdated;
using EM.Catalog.Application.Interfaces;
using EM.Catalog.Domain.DTOs;
using EM.Catalog.Domain.Entities;
using EM.Catalog.Domain.Interfaces;
using MediatR;

namespace EM.Catalog.Infraestructure.Persistense.Decorators;

public class CategoryRepositoryDecorator : ICategoryRepository
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMediator _mediator;

    public CategoryRepositoryDecorator(
        ICategoryRepository categoryRepository,
        IUnitOfWork unitOfWork,
        IMediator mediator)
    {
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
        _mediator = mediator;
    }

    public async Task AddCategoryAsync(Category category)
    {
        await _categoryRepository.AddCategoryAsync(category);

        if (await _unitOfWork.CommitAsync())
        {
            await _mediator.Publish((CategoryAddedEvent)category);
        }
    }

    public async Task AddCategoryAsync(CategoryDTO category)
    {
        await _categoryRepository.AddCategoryAsync(category);
    }

    public async Task<IEnumerable<CategoryDTO>> GetAllCategoriesAsync(short page = 1, short pageSize = 10)
    {
        return await _categoryRepository.GetAllCategoriesAsync(page, pageSize);
    }

    public async Task<Category?> GetCategoryByIdAsync(Guid id)
    {
        return await _categoryRepository.GetCategoryByIdAsync(id);
    }

    public async Task UpdateCategoryAsync(Category category)
    {
        await _categoryRepository.UpdateCategoryAsync(category);

        if (await _unitOfWork.CommitAsync())
        {
            await _mediator.Publish((CategoryUpdatedEvent)category);
        }
    }

    public async Task UpdateCategoryAsync(CategoryDTO category)
    {
        await _categoryRepository.UpdateCategoryAsync(category);
    }
}
