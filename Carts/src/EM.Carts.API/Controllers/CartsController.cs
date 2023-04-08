using EM.Carts.Application.UseCases.AddItem;
using Microsoft.AspNetCore.Mvc;

namespace EM.Carts.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CartsController : BaseController
{
    private readonly IAddItemUseCase _addItemUseCase;

    public CartsController(IAddItemUseCase addItemUseCase)
        => _addItemUseCase = addItemUseCase;

    [HttpPost("Item")]
    public async Task<IActionResult> AddItem(AddItemRequest addItemRequest)
    {
        _addItemUseCase.SetPresenter(this);

        await _addItemUseCase.ExecuteAsync(addItemRequest, GetUserId());

        return _actionResult;
    }
}
