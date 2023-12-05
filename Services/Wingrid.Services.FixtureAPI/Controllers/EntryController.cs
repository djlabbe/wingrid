using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wingrid.Services.FixtureAPI.Models;
using Wingrid.Services.FixtureAPI.Models.Dto;
using Wingrid.Services.FixtureAPI.Services;

namespace Wingrid.Services.FixtureAPI.Controllers;

[Route("api/entries")]
public class EntryController(IEntryService entryService, IMapper mapper) : BaseController<EntryController>
{
    private readonly IEntryService _entryService = entryService;
    private readonly IMapper _mapper = mapper;

    [HttpGet]
    [Authorize]
    [Route("{fixtureId}")]
    public async Task<ResponseDto> Get(int fixtureId)
    {
        return await ExecuteActionAsync(async () =>
        {
            var x = User;
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new Exception("Missing or invalid user id for event.");
            var entry = await _entryService.GetAsync(userId, fixtureId);
            return _mapper.Map<EntryDto>(entry);
        });
    }

    [HttpPost]
    public async Task<ResponseDto> Submit(EntryDto ent)
    {
        return await ExecuteActionAsync(async () =>
        {
            var e = _mapper.Map<Entry>(ent);
            var entry = await _entryService.SubmitEntryAsync(e);
            return _mapper.Map<EntryDto>(entry);
        });
    }

}
