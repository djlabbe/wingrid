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
        return await ExecuteActionAsync(async () =>
        {
            ResponseDto response = await _fixturesService.GetFixturesAsync();
            if (response.IsSuccess)
            {
                List<FixtureDto> fixtures = JsonConvert.DeserializeObject<List<FixtureDto>>(Convert.ToString(response.Result) ?? "") ?? throw new Exception("Error deserializeing API response");
                return Ok(fixtures);
            }
            else
            {
                throw new Exception(response.Message);
            }
        });
    }

    [HttpPost]
    public async Task<IActionResult> CreateFixture([FromBody] FixtureDto fixture)
    {
        return await ExecuteActionAsync(async () =>
        {
            ResponseDto response = await _fixturesService.CreateFixtureAsync(fixture);
            if (response.IsSuccess)
            {
                FixtureDto savedFix = JsonConvert.DeserializeObject<FixtureDto>(Convert.ToString(response.Result) ?? "") ?? throw new Exception("Error deserializeing API response");
                return Ok(savedFix);
            }
            else
            {
                throw new Exception(response.Message);
            }
        });
    }
}

