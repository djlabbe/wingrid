using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wingrid.Services.Collector.Models.Dto;
using Wingrid.Services.Collector.Services;

namespace Wingrid.Services.Collector.Controllers;

[Route("api/events")]
[ApiController]
[Authorize]
public class EventsController : BaseController<EventsController>
{
    private readonly IEventsService _eventsService;
    private IMapper _mapper;

    public EventsController(IEventsService eventsService, IMapper mapper)
    {
        _eventsService = eventsService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ResponseDto> Get([FromQuery] int? season, [FromQuery] int? week)
    {
       return await ExecuteActionAsync(async () => {
            var events = await _eventsService.GetEventsAsync(season, week);
            return _mapper.Map<IEnumerable<EventDto>>(events);
       });
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<ResponseDto> Get(string id)
    {
       return await ExecuteActionAsync(async () => {
            var evnt = await _eventsService.GetEventAsync(id);
            return _mapper.Map<EventDto>(evnt);
       });
    }
}