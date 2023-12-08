using Hangfire;
using Hangfire.Console;
using Hangfire.Server;
using Wingrid.Services.EventAPI.Models;
using Wingrid.Services.EventAPI.Services;

namespace Wingrid.Services.EventAPI.Jobs
{
    [AutomaticRetry(Attempts = 0)]
    public class EventsJob(IEspnService espnService, IEventsService eventsService) : IBatchJob
    {
        private readonly IEspnService _espnService = espnService;
        private readonly IEventsService _eventsService = eventsService;

        public static string JobId => "Events";
        public async Task ExecuteAsync(PerformContext? performContext)
        {
            await SyncNflEvents(performContext);
            await SyncNcaaEvents(performContext);
        }

        private async Task SyncNflEvents(PerformContext? performContext)
        {
            performContext.WriteLine("Fetching current NFL season events...");

            var espnEventResponses = await _espnService.GetNflSeasonEvents(2023);
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

        private async Task SyncNcaaEvents(PerformContext? performContext)
        {
            performContext.WriteLine("Fetching current NCAA season events...");

            var espnEventResponses = await _espnService.GetNcaaSeasonEvents(2023, performContext);
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