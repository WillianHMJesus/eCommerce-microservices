using EM.Catalog.API.Models;
using EM.Catalog.Application.Products.Commands.AddProduct;
using EM.Catalog.Application.Products.Commands.UpdateProduct;
using EM.Catalog.Application.Products.Queries.GetAllProducts;
using EM.Catalog.Application.Products.Queries.GetProductById;
using EM.Catalog.Application.Products.Queries.GetProductsByCategoryId;
using EM.Catalog.Application.Results;
using EM.Catalog.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EM.Catalog.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public sealed class ProductsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProductsController(IMediator mediator)
        => _mediator = mediator;

    [HttpPost]
    public async Task<IActionResult> AddAsync(AddProductRequest addProductRequest, CancellationToken cancellationToken)
    {
        Result result = await _mediator.Send((AddProductCommand)addProductRequest, cancellationToken);

        return !result.Success ? 
            BadRequest(result.Errors) : 
            Created(nameof(GetByIdAsync), new { id = result.Data });
    }

    [HttpPut]
    public async Task<IActionResult> UpdateAsync(UpdateProductRequest updateProductRequest, CancellationToken cancellationToken)
    {
        Result result = await _mediator.Send((UpdateProductCommand)updateProductRequest, cancellationToken);

        return !result.Success ?
            BadRequest(result.Errors) :
            NoContent();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        ProductDTO? product = await _mediator.Send(new GetProductByIdQuery(id), cancellationToken);

        return Ok(product);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync(short page, short pageSize, CancellationToken cancellationToken)
    {
        IEnumerable<ProductDTO?> products = await _mediator.Send(new GetAllProductsQuery(page, pageSize), cancellationToken);

        return Ok(products);
    }

    [HttpGet("Category/{categoryId}")]
    public async Task<IActionResult> GetByCategoryIdAsync(Guid categoryId, CancellationToken cancellationToken)
    {
        IEnumerable<ProductDTO?> products = await _mediator.Send(new GetProductsByCategoryIdQuery(categoryId), cancellationToken);

        return Ok(products);
    }
}
