using Microsoft.AspNetCore.Mvc;
using Wingrid.Collector.Services;

namespace Wingrid.Collector.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EventsController : BaseController<EventsController>
{
    private readonly IEventsService _eventsService;

    public EventsController(IEventsService eventsService)
    {
        _eventsService = eventsService;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] int? season)
    {
       return await ExecuteActionAsync(async () => {
            var events = season == null ? await _eventsService.GetEventsAsync() : await _eventsService.GetEventsBySeasonAsync(season.Value);
            return Ok(events);
       });
    }
}