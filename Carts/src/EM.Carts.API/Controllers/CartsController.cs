using EM.Carts.Application.UseCases.AddItem;
using EM.Carts.Application.UseCases.AddItemQuantity;
using EM.Carts.Application.UseCases.GetCartByUserId;
using Microsoft.AspNetCore.Mvc;

namespace EM.Carts.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CartsController : BaseController
{
    private readonly IAddItemUseCase _addItemUseCase;
    private readonly IAddItemQuantityUseCase _addItemQuantityUseCase;
    private readonly IGetCartByUserIdUseCase _getCartByUserIdUseCase;

    public CartsController(
        IAddItemUseCase addItemUseCase,
        IAddItemQuantityUseCase addItemQuantityUseCase,
        IGetCartByUserIdUseCase getCartByUserIdUseCase)
    {
        _addItemUseCase = addItemUseCase;
        _addItemQuantityUseCase = addItemQuantityUseCase;
        _getCartByUserIdUseCase = getCartByUserIdUseCase;
    }

    [HttpPost("Item")]
    public async Task<IActionResult> AddItem(AddItemRequest addItemRequest)
    {
        _addItemUseCase.SetPresenter(this);

        await _addItemUseCase.ExecuteAsync(addItemRequest, GetUserId());

        return _actionResult;
    }

    [HttpPatch("Item/AddQuantity")]
    public async Task<IActionResult> AddItemQuantity(AddItemQuantityRequest addItemQuantityRequest)
    {
        _addItemQuantityUseCase.SetPresenter(this);

        await _addItemQuantityUseCase.ExecuteAsync(addItemQuantityRequest, GetUserId());

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
