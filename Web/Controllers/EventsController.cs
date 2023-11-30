using Microsoft.AspNetCore.Mvc;
using Wingrid.Web.Services;
using Wingrid.Web.Models;
using Newtonsoft.Json;

namespace Wingrid.Web.Controllers;

[Route("api/events")]
public class EventsController(IEventsService eventsService) : BaseController<EventsController>
{
    private readonly IEventsService _eventsService = eventsService;

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] EventQueryParams eventQueryParams)
    {
       return await ExecuteActionAsync(async () => {
            ResponseDto response = await _eventsService.GetEventsAsync(eventQueryParams);
            if (response.IsSuccess)
            {
                List<EventDto> events = JsonConvert.DeserializeObject<List<EventDto>>(Convert.ToString(response.Result) ?? "") ?? [];
                return Ok(events);
            } else
            {
                throw new Exception(response.Message);
            }
       });
    }
}

