using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wingrid.Services.FixtureAPI.Models;
using Wingrid.Services.FixtureAPI.Models.Dto;
using Wingrid.Services.FixtureAPI.Services;

namespace Wingrid.Services.FixtureAPI.Controllers;

[Route("api/fixtures")]
[ApiController]
// [Authorize]
public class FixturesController(IFixturesService fixturesService, IMapper mapper) : BaseController<FixturesController>
{
   private readonly IFixturesService _fixturesService = fixturesService;
   private readonly IMapper _mapper = mapper;

   [HttpGet]
   public async Task<ResponseDto> Get()
   {
      return await ExecuteActionAsync(async () =>
      {
         var fixtures = await _fixturesService.GetFixturesAsync();
         return _mapper.Map<IEnumerable<FixtureDto>>(fixtures);
      });
   }

   [HttpGet]
   [Route("{id}")]
   public async Task<ResponseDto> Get(int id)
   {
      return await ExecuteActionAsync(async () =>
      {
         var fixture = await _fixturesService.GetFixtureAsync(id);
         return _mapper.Map<FixtureDto>(fixture);
      });
   }

   [HttpPost]
   public async Task<ResponseDto> Create([FromBody] FixtureDto fixture)
   {
      return await ExecuteActionAsync(async () =>
      {
         var fix = _mapper.Map<Fixture>(fixture);
         var savedFix = await _fixturesService.CreateFixture(fix);
         return _mapper.Map<FixtureDto>(savedFix);
      });
   }
}