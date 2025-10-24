using EM.Catalog.Application.Categories.Commands.AddCategory;
using EM.Catalog.Application.Categories.Commands.UpdateCategory;
using EM.Catalog.Application.Categories.Queries.GetAllCategories;
using EM.Catalog.Application.Categories.Queries.GetCategoryById;
using EM.Catalog.Application.Categories.Commands.DeleteCategory;
using Microsoft.AspNetCore.Mvc;
using WH.SharedKernel.Mediator;
using EM.Catalog.API.Models;

namespace EM.Catalog.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public sealed class CategoriesController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> AddAsync(CategoryRequest request, CancellationToken cancellationToken)
    {
        var command = new AddCategoryCommand(
            request.Code,
            request.Name,
            request.Description);

        var result = await mediator.Send(command, cancellationToken);

        return !result.Success ?
            BadRequest(result.Errors) :
            Created(nameof(GetByIdAsync), new { id = result.Data });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        DeleteCategoryCommand command = new(id);

        var result = await mediator.Send(command, cancellationToken);

        return !result.Success ?
            BadRequest(result.Errors) :
            NoContent();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAsync(Guid id, CategoryRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateCategoryCommand(
            id,
            request.Code,
            request.Name,
            request.Description);

        var result = await mediator.Send(command, cancellationToken);

        return !result.Success ?
            BadRequest(result.Errors) :
            NoContent();
    }


    [HttpGet]
    public async Task<IActionResult> GetAllAsync(short page, short pageSize, CancellationToken cancellationToken)
    {
        var categories = await mediator.Send(new GetAllCategoriesQuery(page, pageSize), cancellationToken);

        return Ok(categories);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var category = await mediator.Send(new GetCategoryByIdQuery(id), cancellationToken);

        return Ok(category);
    }
}
