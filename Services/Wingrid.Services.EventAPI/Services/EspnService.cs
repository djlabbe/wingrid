using Hangfire.Console;
using Hangfire.Server;
using Wingrid.Services.EventAPI.Models;
using Wingrid.Services.EventAPI.Models.Espn;

namespace Wingrid.Services.EventAPI.Services
{
    public interface IEspnService
    {
        public Task<IEnumerable<EspnTeam>> GetNFLTeams();
        public Task<IEnumerable<EspnTeam>> GetNCAATeams();
        public Task<IEnumerable<NflEventsResponse>> GetNflSeasonEvents(int season);
        public Task<IEnumerable<CollegeEventsResponse>> GetNcaaSeasonEvents(int season, PerformContext? performContext);
    }

    public class EspnService : IEspnService
    {
        static readonly string NFL_TEAMS_URL = "https://sports.core.api.espn.com/v2/sports/football/leagues/nfl/teams?limit=32";
        static readonly string NCAA_TEAMS_URL = "https://sports.core.api.espn.com/v2/sports/football/leagues/college-football/teams?limit=1000";
        static readonly string NFL_SCOREBOARD_URL = "https://site.api.espn.com/apis/site/v2/sports/football/nfl/scoreboard";
        static readonly string NCAA_SCOREBOARD_URL = "https://site.api.espn.com/apis/site/v2/sports/football/college-football/scoreboard";

        static readonly HttpClient _client = new();

        public async Task<IEnumerable<EspnTeam>> GetNFLTeams()
        {
            TeamsResponse response = await _client.GetFromJsonAsync<TeamsResponse>(NFL_TEAMS_URL) ?? throw new Exception("No response from ESPN.");
            var teams = (response?.Items?.Select(item => item.Ref)?.Select(async r => await _client.GetFromJsonAsync<EspnTeam>(r)).Select(t => t.Result!)) ?? throw new Exception("No teams found.");
            return teams;
        }

        public async Task<IEnumerable<EspnTeam>> GetNCAATeams()
        {
            TeamsResponse response = await _client.GetFromJsonAsync<TeamsResponse>(NCAA_TEAMS_URL) ?? throw new Exception("No response from ESPN.");
            var teams = (response?.Items?.Select(item => item.Ref)?.Select(async r => await _client.GetFromJsonAsync<EspnTeam>(r)).Select(t => t.Result!)) ?? throw new Exception("No teams found.");
            return teams;
        }

        public async Task<IEnumerable<NflEventsResponse>> GetNflSeasonEvents(int season)
        {
            List<NflEventsResponse> events = [];
            for (var w = 0; w < 18; w++)
            {
                NflEventsResponse? response = await _client.GetFromJsonAsync<NflEventsResponse>($"{NFL_SCOREBOARD_URL}?dates={season}&seasontype=2&week={w + 1}") ?? throw new Exception("No response from ESPN.");
                if (response != null && response.Events != null) events.Add(response);
            }

            return events;
        }

        public async Task<IEnumerable<CollegeEventsResponse>> GetNcaaSeasonEvents(int season, PerformContext? performContext)
        {
            string fmt = "00";
            List<CollegeEventsResponse> events = [];
            for (var m = 1; m <= 12; m++)
            {
                var query = $"{NCAA_SCOREBOARD_URL}?limit=1000&dates={season}{m.ToString(fmt)}01-{season}{m.ToString(fmt)}{DateTime.DaysInMonth(season, m)}";
                performContext.WriteLine(query);
                CollegeEventsResponse? response = await _client.GetFromJsonAsync<CollegeEventsResponse>(query) ?? throw new Exception("No response from ESPN.");
                if (response != null && response.Events != null) events.Add(response);

            }
            return events;
        }
    }
}