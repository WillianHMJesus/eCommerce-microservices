using EM.Carts.Application.UseCases.AddItem;
using EM.Carts.Application.UseCases.GetCartByUserId;
using Microsoft.AspNetCore.Mvc;

namespace EM.Carts.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CartsController : BaseController
{
    private readonly IAddItemUseCase _addItemUseCase;
    private readonly IGetCartByUserIdUseCase _getCartByUserIdUseCase;

    public CartsController(
        IAddItemUseCase addItemUseCase,
        IGetCartByUserIdUseCase getCartByUserIdUseCase)
    {
        _addItemUseCase = addItemUseCase;
        _getCartByUserIdUseCase = getCartByUserIdUseCase;
    }

    [HttpPost("Item")]
    public async Task<IActionResult> AddItem(AddItemRequest addItemRequest)
    {
        _addItemUseCase.SetPresenter(this);

        await _addItemUseCase.ExecuteAsync(addItemRequest, GetUserId());

        return _actionResult;
    }

    [HttpGet]
    public async Task<IActionResult> GetCart()
    {
        _getCartByUserIdUseCase.SetPresenter(this);

        await _getCartByUserIdUseCase.ExecuteAsync(GetUserId());

        return _actionResult;
    }
}
