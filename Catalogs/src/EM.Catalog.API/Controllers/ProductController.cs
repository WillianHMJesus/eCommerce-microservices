using EM.Catalog.API.Models;
using EM.Catalog.Application.Products.Commands.AddProduct;
using EM.Catalog.Application.Products.Commands.UpdateProduct;
using EM.Catalog.Application.Products.Queries.GetProductById;
using EM.Catalog.Domain.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EM.Catalog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductController(IMediator mediator)
         => _mediator = mediator;

        [HttpPost]
        public async Task<IActionResult> PostAsync(AddProductRequest addProductRequest)
        {
            var response = await _mediator.Send((AddProductCommand)addProductRequest);

            if (!response.Success)
            {
                return BadRequest(response.Errors);
            }

            return Created(nameof(GetByIdAsync), new { id = response.Data });
        }

        [HttpPut]
        public async Task<IActionResult> PutAsync(UpdateProductRequest updateProductRequest)
        {
            var response = await _mediator.Send((UpdateProductCommand)updateProductRequest);

            if (!response.Success)
            {
                return BadRequest(response.Errors);
            }

            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            ProductDTO? product = await _mediator.Send(new GetProductByIdQuery { Id = id });

            return Ok(product);
        }
    }
}
