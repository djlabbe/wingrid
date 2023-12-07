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

            if (fixture.EventIds.Count == 0)
            {
                throw new Exception("Request must contain at least one event.");
            }

            if (invalidIds.Any())
            {
                throw new Exception($"Request contains invalid event ids. ({string.Join(",", invalidIds)})");
            }

            if (!events.Exists(e => e.Id == fixture.TiebreakerEventId))
            {
                throw new Exception("Request contains invalid Tiebreaker event. Tiebreaker event must be included in events.");
            }

            var timesValid = events.All(e => e.TimeValid == true);
            if (!timesValid) throw new Exception("Request contains events which have not been scheduled. Please try again soon.");
            var firstEventStartTime = events.First().Date ?? throw new Exception("Unable to determine fixture deadline based on selected events.");

            var lockTime = firstEventStartTime.AddHours(-2);
            fixture.Deadline = lockTime;

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