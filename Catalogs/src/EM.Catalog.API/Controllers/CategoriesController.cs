﻿using EM.Catalog.API.Models;
using EM.Catalog.Application.Categories.Commands.AddCategory;
using EM.Catalog.Application.Categories.Commands.UpdateCategory;
using EM.Catalog.Application.Categories.Queries.GetAllCategories;
using EM.Catalog.Application.Categories.Queries.GetCategoryById;
using EM.Catalog.Application.Results;
using EM.Catalog.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EM.Catalog.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public sealed class CategoriesController : ControllerBase
{
    private readonly IMediator _mediator;

    public CategoriesController(IMediator mediator)
        => _mediator = mediator;

    [HttpPost]
    public async Task<IActionResult> AddAsync(AddCategoryRequest addCategoryRequest, CancellationToken cancellationToken)
    {
        Result result = await _mediator.Send((AddCategoryCommand)addCategoryRequest, cancellationToken);

        if (!result.Success)
        {
            return BadRequest(result.Errors);
        }

        return Created(nameof(GetByIdAsync), new { id = result.Data });
    }

    [HttpPut]
    public async Task<IActionResult> UpdateAsync(UpdateCategoryRequest updateCategoryRequest, CancellationToken cancellationToken)
    {
        Result result = await _mediator.Send((UpdateCategoryCommand)updateCategoryRequest, cancellationToken);

        if (!result.Success)
        {
            return BadRequest(result.Errors);
        }

        return NoContent();
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
