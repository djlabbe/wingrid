using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Wingrid.Services.FixtureAPI.Models;
using Wingrid.Services.FixtureAPI.Models.Dto;
using Wingrid.Services.FixtureAPI.Services;

namespace Wingrid.Services.FixtureAPI.Controllers;

[Route("api/entry")]
public class EntryController(IEntryService entryService, IMapper mapper) : BaseController<EntryController>
{
    private readonly IEntryService _entryService = entryService;
    private readonly IMapper _mapper = mapper;

    [HttpGet]
    [Route("{id}")]
    public async Task<ResponseDto> Get(int id)
    {
        return await ExecuteActionAsync(async () => {
            var entry = await _entryService.GetAsync(id);
            return _mapper.Map<EntryDto>(entry);
        });
    }

    [HttpPost]
    public async Task<ResponseDto> Submit(EntryDto ent)
    {
        return await ExecuteActionAsync(async () => {
            var e = _mapper.Map<Entry>(ent);
            var entry = await _entryService.SubmitEntryAsync(e);
            return _mapper.Map<EntryDto>(entry);
        });
    }

}
