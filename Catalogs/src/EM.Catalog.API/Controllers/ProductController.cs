using EM.Catalog.API.Models;
using EM.Catalog.Application.Products.Commands.AddProduct;
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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            return NoContent();
        }
    }
}
