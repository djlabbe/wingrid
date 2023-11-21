using Domain.Exceptions;
using Domain;
using Domain.Espn;

namespace Collector.Services
{
    public interface IEspnService
    {
        public Task<IEnumerable<Team>> GetTeams();
        public Task<IEnumerable<EventsResponse>> GetSeasonEvents(string season);
        public Task<IEnumerable<EspnEvent>> GetEvents(string year, string week);
        // public Task<IEnumerable<Event>> GetCurrentEvents();
        // public Task<Score> GetScore(string eventId, string competitionId, string competitorId);
    }

    public class EspnService : IEspnService
    {   
        static readonly string ESPN_CORE_API = "https://sports.core.api.espn.com/v2/sports/football/leagues/nfl";
        static readonly string ESPN_SITE_API = "https://site.api.espn.com/apis/site/v2/sports/football/nfl";

        //  https://site.api.espn.com/apis/site/v2/sports/football/nfl/scoreboard?dates=2023&seasontype=2&week=17
        //  https://site.api.espn.com/apis/site/v2/sports/football/nfl/scoreboard?dates=2023&seasontype=2&week=1
        //  https://site.api.espn.com/apis/site/v2/sports/football/nfl/scoreboard?dates=2023&seasontype=2&week=17
        
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

        // public async Task<IEnumerable<Event>> GetCurrentEvents()
        // {
        //     EventsResponse response = await _client.GetFromJsonAsync<EventsResponse>($"{ESPN_CORE_API}/events") ?? throw new NotFoundException();
        //     var events = (response?.Items?.Select(item => item.Ref)?.Select(async r => await _client.GetFromJsonAsync<Event>(r)).Select(x => x.Result!)) ?? throw new Exception("No current events found.");
        //     return events;
        // }

        // public async Task<Score> GetScore(string eventId, string competitionId, string competitorId)
        // {
        //     Score response = await _client.GetFromJsonAsync<Score>($"{ESPN_CORE_API}/events/{eventId}/competitions/{competitionId}/competitors/{competitorId}/score?lang=en&region=us") ?? throw new NotFoundException();
        //     return response;
        // }
    }
}