using EM.Catalog.Application.Categories.Commands.AddCategory;
using EM.Catalog.Application.Categories.Commands.UpdateCategory;
using EM.Catalog.Application.Categories.Queries.GetAllCategories;
using EM.Catalog.Application.Categories.Queries.GetCategoryById;
using EM.Catalog.Application.Categories.Commands.DeleteCategory;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using EM.Catalog.Application.Categories.Models;
using AutoMapper;
using EM.Common.Core.ResourceManagers;

namespace EM.Catalog.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public sealed class CategoriesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public CategoriesController(
        IMediator mediator,
        IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    #region Commands
    [HttpPost]
    public async Task<IActionResult> AddAsync(CategoryRequest addCategoryRequest, CancellationToken cancellationToken)
    {
        AddCategoryCommand addCategoryCommand = _mapper.Map<AddCategoryCommand>(addCategoryRequest);
        Result result = await _mediator.Send(addCategoryCommand, cancellationToken);

        return !result.Success ?
            BadRequest(result.Errors) :
            Created(nameof(GetByIdAsync), new { id = result.Data });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        DeleteCategoryCommand deleteCategoryCommand = new(id);
        Result result = await _mediator.Send(deleteCategoryCommand, cancellationToken);

        return !result.Success ?
            BadRequest(result.Errors) :
            NoContent();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAsync(Guid id, CategoryRequest categoryRequest, CancellationToken cancellationToken)
    {
        UpdateCategoryCommand updateCategoryCommand = _mapper.Map<UpdateCategoryCommand>((id, categoryRequest));
        Result result = await _mediator.Send(updateCategoryCommand, cancellationToken);

        return !result.Success ?
            BadRequest(result.Errors) :
            NoContent();
    }
    #endregion

    #region Queries
    [HttpGet]
    public async Task<IActionResult> GetAllAsync(short page, short pageSize, CancellationToken cancellationToken)
    {
        IEnumerable<CategoryDTO?> categories = 
            await _mediator.Send(new GetAllCategoriesQuery(page, pageSize), cancellationToken);

        return Ok(categories);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        CategoryDTO? category = 
            await _mediator.Send(new GetCategoryByIdQuery(id), cancellationToken);

        return Ok(category);
    }
    #endregion
}
