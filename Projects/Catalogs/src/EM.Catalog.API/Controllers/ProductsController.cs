using EM.Catalog.Application.Products.Commands.AddProduct;
using EM.Catalog.Application.Products.Commands.UpdateProduct;
using EM.Catalog.Application.Products.Queries.GetAllProducts;
using EM.Catalog.Application.Products.Queries.GetProductById;
using EM.Catalog.Application.Products.Queries.GetProductsByCategoryId;
using EM.Catalog.Application.Products.Commands.DeleteProduct;
using EM.Catalog.Application.Products.Commands.MakeAvailableProduct;
using EM.Catalog.Application.Products.Commands.MakeUnavailableProduct;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using EM.Catalog.Application.Products.Models;
using AutoMapper;
using EM.Common.Core.ResourceManagers;
using EM.Catalog.Application.Products.Queries.SearchProducts;

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

    #region Commands
    [HttpPost]
    public async Task<IActionResult> AddAsync(ProductRequest productRequest, CancellationToken cancellationToken)
    {
        AddProductCommand addProductCommand = _mapper.Map<AddProductCommand>(productRequest);
        Result result = await _mediator.Send(addProductCommand, cancellationToken);

        return !result.Success ? 
            BadRequest(result.Errors) : 
            Created(nameof(GetByIdAsync), new { id = result.Data });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        DeleteProductCommand deleteProductCommand = new(id);
        Result result = await _mediator.Send(deleteProductCommand, cancellationToken);

        return !result.Success ?
            BadRequest(result.Errors) :
            NoContent();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAsync(Guid id, ProductRequest productRequest, CancellationToken cancellationToken)
    {
        UpdateProductCommand updateProductCommand = _mapper.Map<UpdateProductCommand>((id, productRequest));
        Result result = await _mediator.Send(updateProductCommand, cancellationToken);

        return !result.Success ?
            BadRequest(result.Errors) :
            NoContent();
    }

    [HttpPatch("make-available/{id}")]
    public async Task<IActionResult> MakeAvailableAsync(Guid id, CancellationToken cancellationToken)
    {
        MakeAvailableProductCommand makeAvailableProductCommand = new(id);
        Result result = await _mediator.Send(makeAvailableProductCommand, cancellationToken);

        return !result.Success ?
            BadRequest(result.Errors) :
            NoContent();
    }

    [HttpPatch("make-unavailable/{id}")]
    public async Task<IActionResult> MakeUnavailableAsync(Guid id, CancellationToken cancellationToken)
    {
        MakeUnavailableProductCommand makeunavailableProductCommand = new(id);
        Result result = await _mediator.Send(makeunavailableProductCommand, cancellationToken);

        return !result.Success ?
            BadRequest(result.Errors) :
            NoContent();
    }
    #endregion

    #region Queries
    [HttpGet]
    public async Task<IActionResult> GetAllAsync(short page, short pageSize, CancellationToken cancellationToken)
    {
        IEnumerable<ProductDTO?> products = 
            await _mediator.Send(new GetAllProductsQuery(page, pageSize), cancellationToken);

        return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        ProductDTO? product = 
            await _mediator.Send(new GetProductByIdQuery(id), cancellationToken);

        return Ok(product);
    }

    [HttpGet("Category/{categoryId}")]
    public async Task<IActionResult> GetByCategoryIdAsync(Guid categoryId, short page, short pageSize, CancellationToken cancellationToken)
    {
        IEnumerable<ProductDTO?> products = 
            await _mediator.Send(new GetProductsByCategoryIdQuery(categoryId, page, pageSize), cancellationToken);

        return Ok(products);
    }

    [HttpGet("Search/{text}")]
    public async Task<IActionResult> SearchAsync(string text, short page, short pageSize, CancellationToken cancellationToken)
    {
        IEnumerable<ProductDTO?> products = 
            await _mediator.Send(new SearchProductsQuery(text, page, pageSize), cancellationToken);

        return Ok(products);
    }
    #endregion
}
