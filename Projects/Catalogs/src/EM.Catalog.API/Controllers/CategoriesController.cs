using EM.Catalog.Application.Categories.Commands.AddCategory;
using EM.Catalog.Application.Categories.Commands.UpdateCategory;
using EM.Catalog.Application.Categories.Queries.GetAllCategories;
using EM.Catalog.Application.Categories.Queries.GetCategoryById;
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

    [HttpPost]
    public async Task<IActionResult> AddAsync(AddCategoryRequest addCategoryRequest, CancellationToken cancellationToken)
    {
        AddCategoryCommand addCategoryCommand = _mapper.Map<AddCategoryCommand>(addCategoryRequest);
        Result result = await _mediator.Send(addCategoryCommand, cancellationToken);

        return !result.Success ?
            BadRequest(result.Errors) :
            Created(nameof(GetByIdAsync), new { id = result.Data });
    }

    [HttpPut]
    public async Task<IActionResult> UpdateAsync(UpdateCategoryRequest updateCategoryRequest, CancellationToken cancellationToken)
    {
        UpdateCategoryCommand updateCategoryCommand = _mapper.Map<UpdateCategoryCommand>(updateCategoryRequest);
        Result result = await _mediator.Send(updateCategoryCommand, cancellationToken);

        return !result.Success ? 
            BadRequest(result.Errors) : 
            NoContent();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        CategoryDTO? category = await _mediator.Send(new GetCategoryByIdQuery(id), cancellationToken);
        
        return Ok(category);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync(short page, short pageSize, CancellationToken cancellationToken)
    {
        IEnumerable<CategoryDTO?> categories = await _mediator.Send(new GetAllCategoriesQuery(page, pageSize), cancellationToken);

        return Ok(categories);
    }
}
