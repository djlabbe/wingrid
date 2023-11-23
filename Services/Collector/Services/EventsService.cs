using Microsoft.EntityFrameworkCore;
using Wingrid.Services.Collector.Data;
using Wingrid.Services.Collector.Models;
using Wingrid.Services.Collector.Models.Espn;

namespace Wingrid.Services.Collector.Services
{
    public interface IEventsService
    {
        public Task<IEnumerable<Event>> GetEventsAsync(int? season, int? week);
        public Task<Event?> GetEventAsync(string id);
        public Task<Event> AddOrUpdateEventAsync(EspnEvent espnEvent);
        public Task<int> SaveChangesAsync();
    }

    public class EventsService : IEventsService
    {
        private readonly AppDbContext _context;

        public EventsService(AppDbContext context)
        {
            _context = context;
        }


        public async Task<IEnumerable<Event>> GetEventsAsync(int? season, int? week)
        {
            if (season == null && week == null) {
                 var events = await _context.Events.Include(e => e.HomeTeam).Include(e => e.AwayTeam).OrderBy(e => e.Date).ToListAsync();
                return events;
            } else if (week == null) {
                var events = await _context.Events.Where(e => e.Season == season).Include(e => e.HomeTeam).Include(e => e.AwayTeam).OrderBy(e => e.Date).ToListAsync();
                return events;
            } else if (season == null) {
                var events = await _context.Events.Where(e => e.Week == week).Include(e => e.HomeTeam).Include(e => e.AwayTeam).OrderBy(e => e.Date).ToListAsync();
                return events;
            } else {
                 var events = await _context.Events.Where(e => e.Season == season && e.Week == week).Include(e => e.HomeTeam).Include(e => e.AwayTeam).OrderBy(e => e.Date).ToListAsync();
                return events;
            }
        }

        public Task<Event?> GetEventAsync(string id)
        {
            var evnt = _context.Events.FirstOrDefaultAsync(e => e.Id == id);
            return evnt;
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