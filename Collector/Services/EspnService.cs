using Wingrid.Collector.Models;
using Wingrid.Collector.Models.Espn;
using Wingrid.Collector.Models.Exceptions;

namespace Wingrid.Collector.Services
{
    public interface IEspnService
    {
        public Task<IEnumerable<Team>> GetTeams();
        public Task<IEnumerable<EventsResponse>> GetSeasonEvents(string season);
        public Task<IEnumerable<EspnEvent>> GetEvents(string year, string week);
    }

    public class EspnService : IEspnService
    {   
        static readonly string ESPN_CORE_API = "https://sports.core.api.espn.com/v2/sports/football/leagues/nfl";
        static readonly string ESPN_SITE_API = "https://site.api.espn.com/apis/site/v2/sports/football/nfl";

        static readonly HttpClient _client = new();

        public async Task<IEnumerable<Team>> GetTeams()
        {
            TeamsResponse response = await _client.GetFromJsonAsync<TeamsResponse>($"{ESPN_CORE_API}/teams?limit=32") ?? throw new NotFoundException();
            var teams = (response?.Items?.Select(item => item.Ref)?.Select(async r => await _client.GetFromJsonAsync<Team>(r)).Select(t => t.Result!)) ?? throw new Exception("No teams found.");
            return teams;
        }

        public async Task<IEnumerable<EventsResponse>> GetSeasonEvents(string season)
        {   
            List<EventsResponse> events = new();
            for (var w = 0; w < 18; w++)
            {
                EventsResponse? response = await _client.GetFromJsonAsync<EventsResponse>($"{ESPN_SITE_API}/scoreboard?dates={season}&seasontype=2&week={w + 1}") ?? throw new NotFoundException();
                if (response != null && response.Events != null ) events.Add(response);
            }
           
            return events;
        }

        public async Task<IEnumerable<EspnEvent>> GetEvents(string year, string week)
        {
            EventsResponse? response = await _client.GetFromJsonAsync<EventsResponse>($"{ESPN_SITE_API}/scoreboard?dates={year}&seasontype=2&week={week}") ?? throw new NotFoundException();
            if (response != null && response.Events != null ) return response.Events;
            return new List<EspnEvent>();
        }
    }
}