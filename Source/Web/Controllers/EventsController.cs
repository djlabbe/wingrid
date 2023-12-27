using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wingrid.Models;
using Wingrid.Models.Dto;
using Wingrid.Services;

namespace Wingrid.Controllers;

[Route("api/events")]
[ApiController]
// [Authorize]
public class EventsController(IEventsService eventsService, IMapper mapper) : BaseController<EventsController>
{
    private readonly IEventsService _eventsService = eventsService;
    private readonly IMapper _mapper = mapper;

    [HttpGet]
    public async Task<ResponseDto> Get([FromQuery] EventQueryParams eventQueryParams)
    {
        return await ExecuteActionAsync(async () =>
        {
            var events = await _eventsService.GetEventsAsync(eventQueryParams);
            return _mapper.Map<IEnumerable<EventDto>>(events);
        });
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<ResponseDto> Get(string id)
    {
        return await ExecuteActionAsync(async () =>
        {
            var evnt = await _eventsService.GetEventAsync(id);
            return _mapper.Map<EventDto>(evnt);
        });
    }
}