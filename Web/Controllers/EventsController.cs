﻿using Microsoft.AspNetCore.Mvc;
using Wingrid.Web.Services;

namespace Wingrid.Web.Controllers;

[Route("api/[controller]")]
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
            var events = await _eventsService.GetEventsAsync(season);
            return Ok(events);
       });
    }
}
