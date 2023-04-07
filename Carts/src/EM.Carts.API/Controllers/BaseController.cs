using EM.Carts.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EM.Carts.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class BaseController : ControllerBase, IPresenter
{
    protected IActionResult _actionResult = null!;

    public void Success(object? data = null)
        => _actionResult = Ok(data);

    void IBadRequest.BadRequest(object erros)
        => _actionResult = BadRequest(erros);
}
