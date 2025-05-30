﻿using Microsoft.AspNetCore.Mvc;

namespace EM.Checkout.API.Controllers;

public abstract class BaseController : ControllerBase
{
    protected IActionResult _actionResult = null!;

    [NonAction]
    public Guid GetUserId()
        => Guid.Parse("73e84dc0-5da5-4da6-80c6-8c37d211ba1b");
}
