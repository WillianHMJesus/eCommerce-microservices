using EM.Carts.Application.Interfaces.UseCases;
using EM.Carts.Application.UseCases.AddItem;
using EM.Carts.Application.UseCases.AddItemQuantity;
using EM.Carts.Application.UseCases.DeleteAllItems;
using EM.Carts.Application.UseCases.DeleteItem;
using EM.Carts.Application.UseCases.GetCartByUserId;
using EM.Carts.Application.UseCases.RemoveItemQuantity;
using Microsoft.AspNetCore.Mvc;

namespace EM.Carts.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public sealed class CartsController : BaseController
{
    private readonly IUseCase<AddItemRequest> _addItemUseCase;
    private readonly IUseCase<AddItemQuantityRequest> _addItemQuantityUseCase;
    private readonly IUseCase<RemoveItemQuantityRequest> _subtractItemQuantityUseCase;
    private readonly IUseCase<DeleteItemRequest> _deleteItemUseCase;
    private readonly IUseCase<DeleteAllItemsRequest> _deleteAllItemsUseCase;
    private readonly IUseCase<GetCartByUserIdRequest> _getCartByUserIdUseCase;

    public CartsController(
        IUseCase<AddItemRequest> addItemUseCase,
        IUseCase<AddItemQuantityRequest> addItemQuantityUseCase,
        IUseCase<RemoveItemQuantityRequest> subtractItemQuantityUseCase,
        IUseCase<DeleteItemRequest> deleteItemUseCase,
        IUseCase<DeleteAllItemsRequest> deleteAllItemsUseCase,
        IUseCase<GetCartByUserIdRequest> getCartByUserIdUseCase)
    {
        _addItemUseCase = addItemUseCase;
        _addItemQuantityUseCase = addItemQuantityUseCase;
        _subtractItemQuantityUseCase = subtractItemQuantityUseCase;
        _deleteItemUseCase = deleteItemUseCase;
        _deleteAllItemsUseCase = deleteAllItemsUseCase;
        _getCartByUserIdUseCase = getCartByUserIdUseCase;
    }

    [HttpPost("Item")]
    public async Task<IActionResult> AddItemAsync(AddItemRequest request, CancellationToken cancellationToken)
    {
        _addItemUseCase.SetPresenter(this);
        request.UserId = GetUserId();

        await _addItemUseCase.ExecuteAsync(request, cancellationToken);

        return _actionResult;
    }

    [HttpPatch("Item/AddQuantity/{productId}")]
    public async Task<IActionResult> AddItemQuantityAsync(Guid productId, AddItemQuantityRequest request, CancellationToken cancellationToken)
    {
        _addItemQuantityUseCase.SetPresenter(this);
        request.UserId = GetUserId();
        request.ProductId = productId;

        await _addItemQuantityUseCase.ExecuteAsync(request, cancellationToken);

        return _actionResult;
    }

    [HttpPatch("Item/RemoveQuantity/{productId}")]
    public async Task<IActionResult> RemoveItemQuantityAsync(Guid productId, RemoveItemQuantityRequest request, CancellationToken cancellationToken)
    {
        _subtractItemQuantityUseCase.SetPresenter(this);
        request.UserId = GetUserId();
        request.ProductId = productId;

        await _subtractItemQuantityUseCase.ExecuteAsync(request, cancellationToken);

        return _actionResult;
    }

    [HttpDelete("Item/{productId}")]
    public async Task<IActionResult> DeleteItemAsync(Guid productId, CancellationToken cancellationToken)
    {
        _deleteItemUseCase.SetPresenter(this);
        DeleteItemRequest request = new()
        {
            UserId = GetUserId(),
            ProductId = productId
        };

        await _deleteItemUseCase.ExecuteAsync(request, cancellationToken);

        return _actionResult;
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteAllItemsAsync(CancellationToken cancellationToken)
    {
        _deleteAllItemsUseCase.SetPresenter(this);
        DeleteAllItemsRequest request = new() { UserId = GetUserId() };

        await _deleteAllItemsUseCase.ExecuteAsync(request, cancellationToken);

        return _actionResult;
    }

    [HttpGet]
    public async Task<IActionResult> GetCartAsync(CancellationToken cancellationToken)
    {
        _getCartByUserIdUseCase.SetPresenter(this);
        GetCartByUserIdRequest request = new() { UserId = GetUserId() };

        await _getCartByUserIdUseCase.ExecuteAsync(request, cancellationToken);

        return _actionResult;
    }
}
