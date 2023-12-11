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
        public Task<Event> AddOrUpdateEventAsync(League league, EspnEvent espnEvent);
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
            var start = eventQueryParams.Start;
            var end = eventQueryParams.End;
            var ids = eventQueryParams.Id?.Split(",") ?? [];

            List<Event> events;
            if (start == null && end == null)
            {
                events = await _context.Events.Include(e => e.HomeTeam).Include(e => e.AwayTeam).OrderBy(e => e.Date).ToListAsync();
            }
            else if (start != null && end == null || start == null && end != null)
            {
                throw new Exception("Must provide both start and end");
            }
            else
            {
                events = await _context.Events.Where(e => e.Date >= start && e.Date <= end).Include(e => e.HomeTeam).Include(e => e.AwayTeam).OrderBy(e => e.Date).ToListAsync();
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

        public async Task<Event> AddOrUpdateEventAsync(League league, EspnEvent espnEvent)
        {
            var evnt = await _context.Events.FirstOrDefaultAsync(e => e.Id == espnEvent.Id);

            if (evnt == null)
            {
                evnt = new Event(espnEvent);

                var homeTeamEspnId = espnEvent.Competitions?.FirstOrDefault()?.Competitors?.FirstOrDefault(c => c.HomeAway == "home")?.Team?.Id;
                var awayTeamEspnId = espnEvent.Competitions?.FirstOrDefault()?.Competitors?.FirstOrDefault(c => c.HomeAway == "away")?.Team?.Id;

                evnt.HomeTeamId = _context.Teams.FirstOrDefault(t => t.League == league && t.EspnId == homeTeamEspnId)?.Id;
                evnt.AwayTeamId = _context.Teams.FirstOrDefault(t => t.League == league && t.EspnId == awayTeamEspnId)?.Id;

                _context.Add(evnt);
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
            var connectionString = _configuration.GetValue<string>("ServiceBusConnectionString") ??
                throw new Exception("Missing configuration for ServiceBusConnectionString");
            var queueName = _configuration.GetValue<string>("TopicAndQueueNames:EventFinalQueue") ??
                throw new Exception("Missing configuration for TopicAndQueueNames:EventFinalQueue");
            await _messageBus.PublishMessage(evnt, connectionString, queueName);
        }

        public Task<int> SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }
    }
}