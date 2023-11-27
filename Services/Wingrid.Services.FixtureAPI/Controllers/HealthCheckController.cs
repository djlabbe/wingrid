using Microsoft.AspNetCore.Mvc;

namespace Wingrid.Services.FixtureAPI.Controllers;

[Route("api/health-check")]
[ApiController]
public class HealthCheckController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok();
    }
}
