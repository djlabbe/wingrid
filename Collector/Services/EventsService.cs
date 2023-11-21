using Domain;
using Domain.Espn;
using Microsoft.EntityFrameworkCore;
using Persistence;
using static Domain.Espn.Status;

namespace Application.Services
{
    public interface IEventsService
    {
        public Task<IEnumerable<Event>> GetEventsAsync();
        public Task<Event> AddOrUpdateEvent(EspnEvent espnEvent);
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

        public async Task<Event> AddOrUpdateEvent(EspnEvent espnEvent)
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