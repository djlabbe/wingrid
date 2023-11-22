using Microsoft.EntityFrameworkCore;
using Wingrid.Collector.Models;
using Wingrid.Collector.Models.Espn;

namespace Wingrid.Collector.Services
{
    public interface IEventsService
    {
        public Task<IEnumerable<Event>> GetEventsAsync();
        public Task<IEnumerable<Event>> GetEventsBySeasonAsync(int season);
        public Task<Event> AddOrUpdateEventAsync(EspnEvent espnEvent);
        public Task<int> SaveChangesAsync();
    }

    public class EventsService : IEventsService
    {
        private readonly DataContext _context;

        public EventsService(DataContext context)
        {
            _context = context;
        }


        public async Task<IEnumerable<Event>> GetEventsAsync()
        {
            var events = await _context.Events.ToListAsync();
            return events;
        }

        public async Task<IEnumerable<Event>> GetEventsBySeasonAsync(int season)
        {
            var events = await _context.Events.Where(e => e.Season == season).ToListAsync();
            return events;
        }


        public async Task<Event> AddOrUpdateEventAsync(EspnEvent espnEvent)
        {
            var entity = await _context.Events.FirstOrDefaultAsync(e => e.Id == espnEvent.Id);

            if (entity == null) {
                entity = _context.Add(new Event(espnEvent)).Entity;
            } else {
                entity.UpdateFrom(espnEvent);
            }

            return entity;
        }

        public Task<int> SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }
    }
}