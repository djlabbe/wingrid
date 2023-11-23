using Microsoft.AspNetCore.Mvc;
using Wingrid.Web.Services;
using Wingrid.Web.Models;
using Newtonsoft.Json;

namespace Wingrid.Web.Controllers;

public class EventsController : Controller
{
    private readonly IEventsService _eventsService;
    
    public EventsController(IEventsService eventsService)
    {
        _eventsService = eventsService;
    }

    public async Task<IActionResult> EventsIndex()
    {
        List<EventDto> events = [];
        ResponseDto response = await _eventsService.GetEventsAsync(2023, null);
        if (response.IsSuccess)
        {
            events = JsonConvert.DeserializeObject<List<EventDto>>(Convert.ToString(response.Result) ?? "") ?? [];
        } else
        {
            TempData["error"] = response.Message;
        }
        return View(events);
    }
}

