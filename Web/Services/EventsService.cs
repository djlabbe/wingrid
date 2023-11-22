using Wingrid.Web.Models;
using Wingrid.Web.Models.Exceptions;

namespace Wingrid.Web.Services
{
    public interface IEventsService
    {
        public Task<IEnumerable<Event>> GetEventsAsync(int? season);
    }

    public class EventsService : IEventsService
    {
        static readonly HttpClient _client = new();

         static readonly string COLLECTOR_API = "https://localhost:7286/api";

        public async Task<IEnumerable<Event>> GetEventsAsync(int? season)
        {
            var query = season == null ? "" : $"?season={season}";
            var response = await _client.GetFromJsonAsync<List<Event>>($"{COLLECTOR_API}/events{query}") ?? throw new NotFoundException();
            return response;
        }
    }
}