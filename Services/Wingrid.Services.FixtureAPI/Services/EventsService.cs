using Newtonsoft.Json;
using Wingrid.Services.FixtureAPI.Models;
using Wingrid.Services.FixtureAPI.Models.Dto;

namespace Wingrid.Services.FixtureAPI.Services
{
    public interface IEventsService
    {
        public Task<List<EventDto>> GetEventsAsync(IEnumerable<string> eventIds);
    }

    // Used to populate the full Event Details on a Fixture from the EventAPI
    public class EventsService(IHttpClientFactory httpClientFactory) : IEventsService
    {
        private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;

        public async Task<List<EventDto>> GetEventsAsync(IEnumerable<string> eventIds)
        {
            var client = _httpClientFactory.CreateClient("Events");
            var response = await client.GetAsync($"/api/events?id={string.Join(",", eventIds)}");
            var apiContent = await response.Content.ReadAsStringAsync();
            var resp = JsonConvert.DeserializeObject<ResponseDto>(apiContent);
            if (resp?.IsSuccess == true)
            {
                return JsonConvert.DeserializeObject<List<EventDto>>(Convert.ToString(resp.Result) ?? "") ?? [];
            }
            return [];
        }
    }
}