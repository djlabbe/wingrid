using Collector.Services;
using Domain;
using Hangfire.Console;
using Hangfire.Server;

namespace Collector.Jobs
{
    public class TeamsJob : IBatchJob
    {
        private readonly IEspnService _espnService;
        private readonly ITeamsService _teamsService;

        public TeamsJob(IEspnService espnService, ITeamsService teamsService)
        {
            _espnService = espnService;
            _teamsService = teamsService;
        }

        public static string JobId => "Teams";
        public async Task ExecuteAsync(PerformContext? performContext)
        {
            performContext.WriteLine("Syncing Teams...");

            var espnTeams = await _espnService.GetTeams();
            var dbTeams = await _teamsService.GetTeamsAsync();

            foreach (var espnTeam in espnTeams)
            {
                if (espnTeam == null) continue;
                var existingTeam = dbTeams.FirstOrDefault(dbTeam => dbTeam.Id == espnTeam.Id);
                if (existingTeam == null)
                    _teamsService.AddTeam(espnTeam);
                else
                    existingTeam = espnTeam;
            }

            var changeCount = await _teamsService.SaveChangesAsync();

            performContext.WriteLine($"Created or modified {changeCount} Teams.");
        }
    }
}