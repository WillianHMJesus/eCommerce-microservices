using EM.Checkout.Application.UseCases.Purchase;
using Microsoft.AspNetCore.Mvc;

namespace EM.Checkout.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public sealed class OrdersController : BaseController
{
    private readonly IPurchaseUseCase _purchaseUseCase;

    public OrdersController(IPurchaseUseCase purchaseUseCase)
    {
        _purchaseUseCase = purchaseUseCase;
    }

    [HttpPost("Purchase")]
    public async Task<IActionResult> Purchase(PurchaseRequest purchaseRequest, CancellationToken cancellationToken)
    {
        _purchaseUseCase.SetPresenter(this);
        purchaseRequest.UserId = GetUserId();

        await _purchaseUseCase.ExecuteAsync(purchaseRequest, cancellationToken);

        return _actionResult;
    }
}
