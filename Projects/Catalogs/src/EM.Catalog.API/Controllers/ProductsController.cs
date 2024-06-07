using EM.Catalog.Application.Products.Commands.AddProduct;
using EM.Catalog.Application.Products.Commands.UpdateProduct;
using EM.Catalog.Application.Products.Queries.GetAllProducts;
using EM.Catalog.Application.Products.Queries.GetProductById;
using EM.Catalog.Application.Products.Queries.GetProductsByCategoryId;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using EM.Catalog.Application.Products.Models;
using AutoMapper;
using EM.Common.Core.ResourceManagers;

namespace EM.Catalog.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public sealed class ProductsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public ProductsController(
        IMediator mediator,
        IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<IActionResult> AddAsync(AddProductRequest addProductRequest, CancellationToken cancellationToken)
    {
        AddProductCommand addProductCommand = _mapper.Map<AddProductCommand>(addProductRequest);
        Result result = await _mediator.Send(addProductCommand, cancellationToken);

        return !result.Success ? 
            BadRequest(result.Errors) : 
            Created(nameof(GetByIdAsync), new { id = result.Data });
    }

    [HttpPut]
    public async Task<IActionResult> UpdateAsync(UpdateProductRequest updateProductRequest, CancellationToken cancellationToken)
    {
        UpdateProductCommand updateProductCommand = _mapper.Map<UpdateProductCommand>(updateProductRequest);
        Result result = await _mediator.Send(updateProductCommand, cancellationToken);

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
