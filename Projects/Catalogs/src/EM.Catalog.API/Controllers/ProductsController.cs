using EM.Catalog.Application.Products.Commands.AddProduct;
using EM.Catalog.Application.Products.Commands.UpdateProduct;
using EM.Catalog.Application.Products.Queries.GetAllProducts;
using EM.Catalog.Application.Products.Queries.GetProductById;
using EM.Catalog.Application.Products.Queries.GetProductsByCategoryId;
using EM.Catalog.Application.Products.Commands.DeleteProduct;
using Microsoft.AspNetCore.Mvc;
using EM.Catalog.Application.Products.Queries.SearchProducts;
using EM.Catalog.Application.Products;
using EM.Catalog.API.Models;
using WH.SharedKernel.Mediator;
using EM.Catalog.Application.Products.Commands.ReactivateProduct;
using EM.Catalog.Application.Products.Commands.InactivateProduct;

namespace EM.Catalog.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public sealed class ProductsController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> AddAsync(ProductRequest request, CancellationToken cancellationToken)
    {
        var command = new AddProductCommand(
            request.Name,
            request.Description,
            request.Value,
            request.Quantity,
            request.Image,
            request.CategoryId);

        var result = await mediator.Send(command, cancellationToken);

        return !result.Success ? 
            BadRequest(result.Errors) : 
            Created(nameof(GetByIdAsync), new { id = result.Data });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var command = new DeleteProductCommand(id);

        var result = await mediator.Send(command, cancellationToken);

        return !result.Success ?
            BadRequest(result.Errors) :
            NoContent();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAsync(Guid id, ProductRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateProductCommand(
            id,
            request.Name,
            request.Description,
            request.Value,
            request.Quantity,
            request.Image,
            request.CategoryId);

        var result = await mediator.Send(command, cancellationToken);

        return !result.Success ?
            BadRequest(result.Errors) :
            NoContent();
    }

    [HttpPatch("reactivate/{id}")]
    public async Task<IActionResult> MakeAvailableAsync(Guid id, CancellationToken cancellationToken)
    {
        var command = new ReactivateProductCommand(id);

        var result = await mediator.Send(command, cancellationToken);

        return !result.Success ?
            BadRequest(result.Errors) :
            NoContent();
    }

    [HttpPatch("inactivate/{id}")]
    public async Task<IActionResult> MakeUnavailableAsync(Guid id, CancellationToken cancellationToken)
    {
        var command = new InactivateProductCommand(id);

        var result = await mediator.Send(command, cancellationToken);

        return !result.Success ?
            BadRequest(result.Errors) :
            NoContent();
    }


    [HttpGet]
    public async Task<IActionResult> GetAllAsync(short page, short pageSize, CancellationToken cancellationToken)
    {
        var products = await mediator.Send(new GetAllProductsQuery(page, pageSize), cancellationToken);

        return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var product = await mediator.Send(new GetProductByIdQuery(id), cancellationToken);

        return Ok(product);
    }

    [HttpGet("Category/{categoryId}")]
    public async Task<IActionResult> GetByCategoryIdAsync(Guid categoryId, short page, short pageSize, CancellationToken cancellationToken)
    {
        var products = await mediator.Send(new GetProductsByCategoryIdQuery(categoryId, page, pageSize), cancellationToken);

        return Ok(products);
    }

    [HttpGet("Search/{text}")]
    public async Task<IActionResult> SearchAsync(string text, short page, short pageSize, CancellationToken cancellationToken)
    {
        var products = await mediator.Send(new SearchProductsQuery(text, page, pageSize), cancellationToken);

        return Ok(products);
    }
}
