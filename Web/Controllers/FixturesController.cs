using Microsoft.AspNetCore.Mvc;
using Wingrid.Web.Services;
using Wingrid.Web.Models;
using Newtonsoft.Json;

namespace Wingrid.Web.Controllers;

[Route("api/fixtures")]
public class FixturesController(IFixturesService fixturesService) : BaseController<FixturesController>
{
    private readonly IFixturesService _fixturesService = fixturesService;

    [HttpGet]
    public async Task<IActionResult> FixturesIndex()
    {
        ResponseDto response = await _fixturesService.GetFixturesAsync();
        if (response.IsSuccess)
        {
            List<FixtureDto> fixtures = JsonConvert.DeserializeObject<List<FixtureDto>>(Convert.ToString(response.Result) ?? "") ?? [];
            return Ok(fixtures);
        } else
        {
           throw new Exception(response.Message);
        }
    }
}

