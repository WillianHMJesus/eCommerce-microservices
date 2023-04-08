using EM.Carts.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EM.Carts.API.Controllers;

public abstract class BaseController : ControllerBase, IPresenter
{
    protected IActionResult _actionResult = null!;

    [NonAction]
    public Guid GetUserId()
    {
        return Guid.Parse("73e84dc0-5da5-4da6-80c6-8c37d211ba1b");
    }

    [NonAction]
    public void Success(object? data = null)
        => _actionResult = Ok(data);

    [NonAction]
    void IBadRequest.BadRequest(object erros)
        => _actionResult = BadRequest(erros);
}
