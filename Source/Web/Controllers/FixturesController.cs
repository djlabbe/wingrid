using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wingrid.Models;
using Wingrid.Models.Dto;
using Wingrid.Services;

namespace Wingrid.Controllers;

[Route("api/fixtures")]
[ApiController]
public class FixturesController(IFixturesService fixturesService, IMapper mapper) : BaseController<FixturesController>
{
   private readonly IFixturesService _fixturesService = fixturesService;
   private readonly IMapper _mapper = mapper;

   [HttpGet]
   [Authorize]
   public async Task<ResponseDto> Get()
   {
      return await ExecuteActionAsync(async () =>
      {
         var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new Exception("Missing or invalid user id.");
         var fixtures = await _fixturesService.GetFixturesAsync(userId);
         return _mapper.Map<IEnumerable<FixtureDto>>(fixtures);
      });
   }

   [HttpGet]
   [Authorize]
   [Route("{id}")]
   public async Task<ResponseDto> Get(int id)
   {
      return await ExecuteActionAsync(async () =>
      {
         var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new Exception("Missing or invalid user id.");
         var fixture = await _fixturesService.GetFixtureAsync(id, userId);
         return _mapper.Map<FixtureDto>(fixture);
      });
   }

   [HttpGet]
   [Authorize]
   [Route("{id}/entry")]
   public async Task<ResponseDto> GetEntryForUser(int id)
   {
      return await ExecuteActionAsync(async () =>
      {
         var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new Exception("Missing or invalid user id for event.");
         var entry = await _fixturesService.GetEntryAsync(userId, id);
         return _mapper.Map<EntryDto>(entry);
      });
   }

   [HttpPost]
   [Authorize]
   public async Task<ResponseDto> Create([FromBody] CreateFixtureDto fixture)
   {
      return await ExecuteActionAsync(async () =>
      {
         var savedFix = await _fixturesService.CreateFixture(fixture);
         return _mapper.Map<FixtureDto>(savedFix);
      });
   }

   [HttpPost]
   [Authorize]
   [Route("submitentry")]
   public async Task<ResponseDto> Submit(EntryDto ent)
   {
      return await ExecuteActionAsync(async () =>
      {
         var e = _mapper.Map<Entry>(ent);
         var entry = await _fixturesService.SubmitEntryAsync(e);
         return _mapper.Map<EntryDto>(entry);
      });
   }
}