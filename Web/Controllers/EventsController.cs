using Domain;
using Microsoft.AspNetCore.Mvc;
using Web.Services;

namespace Web.Controllers;

[Route("api/[controller]")]
public class EventsController : BaseController<EventsController>
{
    private readonly IEventsService _eventsService;

    public EventsController(IEventsService eventsService)
    {
        _eventsService = eventsService;
    }

    [HttpGet]
    public IEnumerable<Event> Get()
    {
       return _eventsService.GetEvents();
    }
}

