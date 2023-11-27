using Microsoft.AspNetCore.Mvc;
using Wingrid.Web.Services;
using Wingrid.Web.Models;
using Newtonsoft.Json;

namespace Wingrid.Web.Controllers;

public class FixturesController : Controller
{
    private readonly IFixturesService _fixturesService;
    
    public FixturesController(IFixturesService fixturesService)
    {
        _fixturesService = fixturesService;
    }

    public async Task<IActionResult> FixturesIndex()
    {
        List<FixtureDto> fixtures = [];
        ResponseDto response = await _fixturesService.GetFixturesAsync();
        if (response.IsSuccess)
        {
            fixtures = JsonConvert.DeserializeObject<List<FixtureDto>>(Convert.ToString(response.Result) ?? "") ?? [];
        } else
        {
            TempData["error"] = response.Message;
        }
        return View(fixtures);
    }
}

