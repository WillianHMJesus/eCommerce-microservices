using EM.Carts.Application.UseCases.AddItem;
using EM.Carts.Application.UseCases.AddItemQuantity;
using EM.Carts.Application.UseCases.DeleteAllItems;
using EM.Carts.Application.UseCases.DeleteItem;
using EM.Carts.Application.UseCases.GetCartByUserId;
using EM.Carts.Application.UseCases.SubtractItemQuantity;
using Microsoft.AspNetCore.Mvc;

namespace EM.Carts.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public sealed class CartsController : BaseController
{
    private readonly IAddItemUseCase _addItemUseCase;
    private readonly IAddItemQuantityUseCase _addItemQuantityUseCase;
    private readonly ISubtractItemQuantityUseCase _subtractItemQuantityUseCase;
    private readonly IDeleteItemUseCase _deleteItemUseCase;
    private readonly IDeleteAllItemsUseCase _deleteAllItemsUseCase;
    private readonly IGetCartByUserIdUseCase _getCartByUserIdUseCase;

    public CartsController(
        IAddItemUseCase addItemUseCase,
        IAddItemQuantityUseCase addItemQuantityUseCase,
        ISubtractItemQuantityUseCase subtractItemQuantityUseCase,
        IDeleteItemUseCase deleteItemUseCase,
        IDeleteAllItemsUseCase deleteAllItemsUseCase,
        IGetCartByUserIdUseCase getCartByUserIdUseCase)
    {
        _addItemUseCase = addItemUseCase;
        _addItemQuantityUseCase = addItemQuantityUseCase;
        _subtractItemQuantityUseCase = subtractItemQuantityUseCase;
        _deleteItemUseCase = deleteItemUseCase;
        _deleteAllItemsUseCase = deleteAllItemsUseCase;
        _getCartByUserIdUseCase = getCartByUserIdUseCase;
    }

    [HttpPost("Item")]
    public async Task<IActionResult> AddItem(AddItemRequest addItemRequest)
    {
        _addItemUseCase.SetPresenter(this);
        addItemRequest.UserId = GetUserId();

        await _addItemUseCase.ExecuteAsync(addItemRequest);

        return _actionResult;
    }

    [HttpPatch("Item/AddQuantity")]
    public async Task<IActionResult> AddItemQuantity(AddItemQuantityRequest addItemQuantityRequest)
    {
        _addItemQuantityUseCase.SetPresenter(this);
        addItemQuantityRequest.UserId = GetUserId();

        await _addItemQuantityUseCase.ExecuteAsync(addItemQuantityRequest);

        return _actionResult;
    }

    [HttpPatch("Item/SubtractQuantity")]
    public async Task<IActionResult> SubtractItemQuantity(SubtractItemQuantityRequest subtractItemQuantityRequest)
    {
        _subtractItemQuantityUseCase.SetPresenter(this);
        subtractItemQuantityRequest.UserId = GetUserId();

        await _subtractItemQuantityUseCase.ExecuteAsync(subtractItemQuantityRequest);

        return _actionResult;
    }

    [HttpDelete("Item")]
    public async Task<IActionResult> DeleteItem(DeleteItemRequest deleteItemRequest)
    {
        _deleteItemUseCase.SetPresenter(this);
        deleteItemRequest.UserId = GetUserId();

        await _deleteItemUseCase.ExecuteAsync(deleteItemRequest);

        return _actionResult;
    }

    [HttpDelete()]
    public async Task<IActionResult> DeleteAllItems()
    {
        _deleteAllItemsUseCase.SetPresenter(this);

        await _deleteAllItemsUseCase.ExecuteAsync(GetUserId());

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
