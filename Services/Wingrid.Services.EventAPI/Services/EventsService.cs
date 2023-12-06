using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Wingrid.MessageBus;
using Wingrid.Services.EventAPI.Data;
using Wingrid.Services.EventAPI.Models;
using Wingrid.Services.EventAPI.Models.Dto;
using Wingrid.Services.EventAPI.Models.Espn;

namespace Wingrid.Services.EventAPI.Services
{
    public interface IEventsService
    {
        public Task<IEnumerable<Event>> GetEventsAsync(EventQueryParams eventQueryParams);
        public Task<IEnumerable<Event>> GetEventsAsync(IEnumerable<string> ids);
        public Task<Event?> GetEventAsync(string id);
        public Task<Event> AddOrUpdateEventAsync(EspnEvent espnEvent);
        public Task<int> SaveChangesAsync();
        public Task DispatchEventComplete(EventDto evnt);
    }

    public class EventsService(AppDbContext context, IMessageBus messageBus, IConfiguration configuration, IMapper mapper) : IEventsService
    {
        private readonly AppDbContext _context = context;
        private readonly IMessageBus _messageBus = messageBus;
        private readonly IConfiguration _configuration = configuration;
        private readonly IMapper _mapper = mapper;

        public async Task<IEnumerable<Event>> GetEventsAsync(EventQueryParams eventQueryParams)
        {
            var season = eventQueryParams.Season;
            var week = eventQueryParams.Week;
            var ids = eventQueryParams.Id?.Split(",") ?? [];

            List<Event> events;
            if (season == null && week == null)
            {
                events = await _context.Events.Include(e => e.HomeTeam).Include(e => e.AwayTeam).OrderBy(e => e.Date).ToListAsync();
            }
            else if (week == null)
            {
                events = await _context.Events.Where(e => e.Season == season).Include(e => e.HomeTeam).Include(e => e.AwayTeam).OrderBy(e => e.Date).ToListAsync();
            }
            else if (season == null)
            {
                events = await _context.Events.Where(e => e.Week == week).Include(e => e.HomeTeam).Include(e => e.AwayTeam).OrderBy(e => e.Date).ToListAsync();
            }
            else
            {
                events = await _context.Events.Where(e => e.Season == season && e.Week == week).Include(e => e.HomeTeam).Include(e => e.AwayTeam).OrderBy(e => e.Date).ToListAsync();
            }

            if (ids.Length != 0)
            {
                events = events.Where(e => ids.Contains(e.Id)).ToList();
            }

            return events;
        }

        public async Task<IEnumerable<Event>> GetEventsAsync(IEnumerable<string> ids)
        {
            var events = await _context.Events.Where(e => ids.Contains(e.Id)).Include(e => e.HomeTeam).Include(e => e.AwayTeam).OrderBy(e => e.Date).ToListAsync();
            return events;
        }

        public Task<Event?> GetEventAsync(string id)
        {
            var evnt = _context.Events.FirstOrDefaultAsync(e => e.Id == id);
            return evnt;
        }

        public async Task<Event> AddOrUpdateEventAsync(EspnEvent espnEvent)
        {
            var evnt = await _context.Events.FirstOrDefaultAsync(e => e.Id == espnEvent.Id);

            if (evnt == null)
            {
                evnt = _context.Add(new Event(espnEvent)).Entity;
            }
            else
            {
                var prevStatus = evnt.StatusCompleted;

                evnt.UpdateFrom(espnEvent);

                // Publish a message that the event is now complete
                if (prevStatus != true && evnt.StatusCompleted == true)
                {
                    await DispatchEventComplete(_mapper.Map<EventDto>(evnt));
                }
            }

            return evnt;
        }

        public async Task DispatchEventComplete(EventDto evnt)
        {
            var queueName = _configuration.GetValue<string>("TopicAndQueueNames:EventFinalQueue") ??
                throw new Exception("Missing configuration for TopicAndQueueNames:EventFinalQueue");
            await _messageBus.PublishMessage(evnt, queueName);
        }

        public Task<int> SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }
    }
}