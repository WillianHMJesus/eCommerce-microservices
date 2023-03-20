using EM.Catalog.API.Models;
using EM.Catalog.Application.Products.Commands.AddProduct;
using EM.Catalog.Application.Products.Commands.UpdateProduct;
using EM.Catalog.Application.Products.Queries.GetAllProducts;
using EM.Catalog.Application.Products.Queries.GetProductById;
using EM.Catalog.Application.Products.Queries.GetProductsByCategoryId;
using EM.Catalog.Application.Results;
using EM.Catalog.Domain.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EM.Catalog.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProductsController(IMediator mediator)
        => _mediator = mediator;

    [HttpPost]
    public async Task<IActionResult> AddAsync(AddProductRequest addProductRequest)
    {
        Result result = await _mediator.Send((AddProductCommand)addProductRequest);

        if (!result.Success)
        {
            return BadRequest(result.Errors);
        }

        return Created(nameof(GetByIdAsync), new { id = result.Data });
    }

    [HttpPut]
    public async Task<IActionResult> UpdateAsync(UpdateProductRequest updateProductRequest)
    {
        Result result = await _mediator.Send((UpdateProductCommand)updateProductRequest);

        if (!result.Success)
        {
            return BadRequest(result.Errors);
        }

        return NoContent();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdAsync(Guid id)
    {
        ProductDTO? product = await _mediator.Send(new GetProductByIdQuery(id));

        return Ok(product);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync(short page, short pageSize)
    {
        IEnumerable<ProductDTO?> products = await _mediator.Send(new GetAllProductsQuery(page, pageSize));

        return Ok(products);
    }

    [HttpGet("Category/{categoryId}")]
    public async Task<IActionResult> GetByCategoryIdAsync(Guid categoryId)
    {
        IEnumerable<ProductDTO?> products = await _mediator.Send(new GetProductsByCategoryIdQuery(categoryId));

        return Ok(products);
    }
}
