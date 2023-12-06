using Microsoft.EntityFrameworkCore;
using Wingrid.Services.FixtureAPI.Data;
using Wingrid.Services.FixtureAPI.Models;

namespace Wingrid.Services.FixtureAPI.Services
{
    public interface IFixturesService
    {
        public Task<Fixture> CreateFixture(Fixture fixture);
        public Task<IEnumerable<Fixture>> GetFixturesAsync();
        public Task<Fixture?> GetFixtureAsync(int id);
    }

    public class FixturesService(AppDbContext context, IEventsService eventsService) : IFixturesService
    {
        private readonly AppDbContext _context = context;
        private readonly IEventsService _eventsService = eventsService;

        public async Task<Fixture> CreateFixture(Fixture fixture)
        {
            var events = await LoadEventsAsync(fixture);

            var invalidIds = fixture.EventIds.Where(id => events.FirstOrDefault(e => e.Id == id) == null);
            if (invalidIds.Any())
            {
                throw new Exception($"Request contains invalid events ids. ({string.Join(",", invalidIds)})");
            }

            if (!events.Exists(e => e.Id == fixture.TiebreakerEventId))
            {
                throw new Exception($"Tiebreaker event must exist in the Fixture.");
            }

            _context.Fixtures.Add(fixture);
            await _context.SaveChangesAsync();

            return fixture;
        }

        public async Task<IEnumerable<Fixture>> GetFixturesAsync()
        {
            return await _context.Fixtures.ToListAsync();
        }

        public async Task<Fixture?> GetFixtureAsync(int id)
        {
            var fixture = await _context.Fixtures.Include(f => f.Entries).FirstOrDefaultAsync(e => e.Id == id);
            if (fixture != null) fixture.Events = await LoadEventsAsync(fixture);
            return fixture;
        }

        private async Task<List<EventDto>> LoadEventsAsync(Fixture fixture)
        {
            var events = await _eventsService.GetEventsAsync(fixture.EventIds);
            return events;
        }
    }
}