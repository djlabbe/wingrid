using Hangfire;
using Hangfire.Console;
using Hangfire.Server;
using Wingrid.Models;
using Wingrid.Services;

namespace Wingrid.Jobs
{
    [AutomaticRetry(Attempts = 0)]
    public class TeamsJob(IEspnService espnService, ITeamsService teamsService) : IBatchJob
    {
        private readonly IEspnService _espnService = espnService;
        private readonly ITeamsService _teamsService = teamsService;

        public static string JobId => "Teams";
        public async Task ExecuteAsync(PerformContext? performContext)
        {
            await SyncNflAsync(performContext);
            await SyncNcaaAsync(performContext);
        }

        private async Task SyncNflAsync(PerformContext? performContext)
        {
            performContext.WriteLine("Syncing NFL Teams...");

            var espnTeams = await _espnService.GetNFLTeams();
            var dbTeams = await _teamsService.GetTeamsByLeagueAsync(League.NFL);

            foreach (var espnTeam in espnTeams)
            {
                var existingTeam = dbTeams.FirstOrDefault(dbTeam => dbTeam.EspnId == espnTeam.Id && dbTeam.League == League.NFL);

                if (existingTeam == null)
                    _teamsService.AddTeam(new Team(League.NFL, espnTeam));
                else
                    existingTeam.UpdateFrom(espnTeam);
            }

            var changeCount = await _teamsService.SaveChangesAsync();

            performContext.WriteLine($"Created or modified {changeCount} NFL Teams.");
        }

        private async Task SyncNcaaAsync(PerformContext? performContext)
        {
            performContext.WriteLine("Syncing NCAA Teams...");

            var espnTeams = await _espnService.GetNCAATeams();
            var dbTeams = await _teamsService.GetTeamsByLeagueAsync(League.NCAA);

            foreach (var espnTeam in espnTeams)
            {
                var existingTeam = dbTeams.FirstOrDefault(dbTeam => dbTeam.EspnId == espnTeam.Id && dbTeam.League == League.NCAA);

                if (existingTeam == null)
                    _teamsService.AddTeam(new Team(League.NCAA, espnTeam));
                else
                    existingTeam.UpdateFrom(espnTeam);
            }

            var changeCount = await _teamsService.SaveChangesAsync();

            performContext.WriteLine($"Created or modified {changeCount} NCAA Teams.");
        }
    }
}