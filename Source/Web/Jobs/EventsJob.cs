using Hangfire;
using Hangfire.Console;
using Hangfire.Server;
using Wingrid.Models;
using Wingrid.Services;

namespace Wingrid.Jobs
{
    [AutomaticRetry(Attempts = 0)]
    public class EventsJob(IEspnService espnService, IEventsService eventsService) : IBatchJob
    {
        private readonly IEspnService _espnService = espnService;
        private readonly IEventsService _eventsService = eventsService;

        public static string JobId => "Events";
        public async Task ExecuteAsync(PerformContext? performContext)
        {
            var currentYear = DateTime.UtcNow.Year;

            await SyncNflEvents(currentYear, performContext);
            await SyncNcaaEvents(currentYear - 1, performContext); // Temp, find better way than 3 sep calls?
            await SyncNcaaEvents(currentYear, performContext);
            await SyncNcaaEvents(currentYear + 1, performContext);
        }

        private async Task SyncNflEvents(int year, PerformContext? performContext)
        {
            performContext.WriteLine("Fetching current NFL season events...");

            var espnEventResponses = await _espnService.GetNflSeasonEvents(2025);
            performContext.WriteLine($"Retrieved {espnEventResponses.Count()} responses from ESPN.");

            foreach (var response in espnEventResponses)
            {
                performContext.WriteLine($"Processing response for season {response?.Season?.Year} week {response?.Week?.Number}");

                if (response?.Events != null)
                {
                    foreach (var e in response.Events)
                    {
                        performContext.WriteLine($"Processing event {e.Id}");
                        await _eventsService.AddOrUpdateEventAsync(League.NFL, e);
                    }
                }
            }

            var changeCount = await _eventsService.SaveChangesAsync();

            performContext.WriteLine($"Created or modified {changeCount} Events.");
        }

        private async Task SyncNcaaEvents(int year, PerformContext? performContext)
        {
            performContext.WriteLine($"Fetching NCAA events for {year}...");
            var espnEventResponses = await _espnService.GetNcaaSeasonEvents(year, performContext);

            performContext.WriteLine($"Retrieved {espnEventResponses.Count()} responses from ESPN.");
            foreach (var response in espnEventResponses)
            {
                performContext.WriteLine($"Processing response...");

                if (response?.Events != null)
                {
                    foreach (var e in response.Events)
                    {
                        performContext.WriteLine($"Processing event {e.Id}");
                        await _eventsService.AddOrUpdateEventAsync(League.NCAA, e);
                    }
                }
            }

            var changeCount = await _eventsService.SaveChangesAsync();

            performContext.WriteLine($"Created or modified {changeCount} Events.");
        }
    }
}