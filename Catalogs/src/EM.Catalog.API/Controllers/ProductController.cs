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
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Post(AddProductRequest addProductRequest)
        {
            await _mediator.Send((AddProductCommand)addProductRequest);

            return NoContent();
        }
    }
}
