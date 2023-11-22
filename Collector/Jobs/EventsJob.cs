using Hangfire.Console;
using Hangfire.Server;
using Wingrid.Collector.Services;

namespace Wingrid.Collector.Jobs
{
    public class EventsJob : IBatchJob
    {
        private readonly IEspnService _espnService;
        private readonly IEventsService _eventsService;

        public EventsJob(IEspnService espnService, IEventsService eventsService)
        {
            _espnService = espnService;
            _eventsService = eventsService;
        }

        public static string JobId => "Events";
        public async Task ExecuteAsync(PerformContext? performContext)
        {
            performContext.WriteLine("Fetching current season events...");

            var espnEventResponses = await _espnService.GetSeasonEvents("2023");
            performContext.WriteLine($"Retrieved {espnEventResponses.Count()} responses from ESPN.");

            foreach (var response in espnEventResponses)
            {
                performContext.WriteLine($"Processing response for season {response?.Season?.Year} week {response?.Week?.Number}");

                if (response?.Events != null)
                {
                    foreach (var e in response.Events)
                    {
                        performContext.WriteLine($"Processing event {e.Id}");
                        await _eventsService.AddOrUpdateEventAsync(e);
                    }
                }
            }

            var changeCount = await _eventsService.SaveChangesAsync();

            performContext.WriteLine($"Created or modified {changeCount} Events.");
        }
    }
}