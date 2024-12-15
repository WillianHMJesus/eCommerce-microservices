using AutoMapper;
using EM.Checkout.Application.Models;
using EM.Checkout.Application.Orders.Commands.FinishOrder;
using EM.Common.Core.ResourceManagers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EM.Checkout.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public sealed class OrdersController : BaseController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public OrdersController(
        IMediator mediator, 
        IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpPost("Purchase")]
    public async Task<IActionResult> Purchase(FinishOrderRequest request, CancellationToken cancellationToken)
    {
        FinishOrderCommand command = _mapper.Map<FinishOrderCommand>((request, GetUserId()));
        Result result = await _mediator.Send(command, cancellationToken);

        return !result.Success ?
            BadRequest(result.Errors) :
            Accepted();
    }
}
