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
public class CartsController : BaseController
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

    [HttpPatch("Item/SubtractQuantity")]
    public async Task<IActionResult> SubtractItemQuantity(SubtractItemQuantityRequest subtractItemQuantityRequest)
    {
        _subtractItemQuantityUseCase.SetPresenter(this);

        await _subtractItemQuantityUseCase.ExecuteAsync(subtractItemQuantityRequest, GetUserId());

        return _actionResult;
    }

    [HttpDelete("Item")]
    public async Task<IActionResult> DeleteItem(DeleteItemRequest deleteItemRequest)
    {
        _deleteItemUseCase.SetPresenter(this);

        await _deleteItemUseCase.ExecuteAsync(deleteItemRequest, GetUserId());

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
