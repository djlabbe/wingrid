using Microsoft.AspNetCore.Mvc;

namespace Wingrid.Services.EventAPI.Controllers;

[Route("api/events-health-check")]
[ApiController]
public class HealthCheckController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok();
    }
}
