using Microsoft.AspNetCore.Mvc;

namespace MzadPalestine.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class BaseApiController : ControllerBase
{
    protected ActionResult<T> HandleResult<T>(T? result)
    {
        if (result == null)
            return NotFound();

        return Ok(result);
    }
}
