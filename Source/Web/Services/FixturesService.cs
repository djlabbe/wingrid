using Microsoft.EntityFrameworkCore;
using Wingrid.Data;
using Wingrid.Models;
using Wingrid.Models.Dto;

namespace Wingrid.Services
{
    public interface IFixturesService
    {
        public Task<Fixture> CreateFixture(CreateFixtureDto fixture);
        public Task<IEnumerable<Fixture>> GetFixturesAsync(string userId);
        public Task<Fixture?> GetFixtureAsync(int id, string userId);
        public Task<Entry> SubmitEntryAsync(Entry entry);
        public Task<Entry?> GetEntryAsync(string userId, int fixtureId);

    }

    public class FixturesService(AppDbContext context) : IFixturesService
    {
        private readonly AppDbContext _context = context;

        public async Task<Fixture> CreateFixture(CreateFixtureDto fixture)
        {
            var events = await _context.Events.Where(e => fixture.EventIds.Contains(e.Id)).OrderBy(e => e.Date).ToListAsync();
            var invalidIds = fixture.EventIds.Where(id => events.FirstOrDefault(e => e.Id == id) == null);

            if (fixture.EventIds.Count == 0)
            {
                throw new Exception("Fixture must contain at least one event.");
            }

            if (invalidIds.Any())
            {
                throw new Exception($"Fixture contains invalid event ids. ({string.Join(",", invalidIds)})");
            }

            if (!events.Any(e => e.Id == fixture.TiebreakerEventId))
            {
                throw new Exception("Fixture contains invalid Tiebreaker event. Tiebreaker event must be included in events.");
            }

            if (events.Any(e => e.Date < DateTime.Now))
            {
                throw new Exception("Fixture contains events which are already completed or in-progress. All events must be in the future.");
            }

            var timesValid = events.All(e => e.TimeValid == true);
            if (!timesValid) throw new Exception("Fixture contains events which have not been scheduled. Please try again soon.");

            var firstEventStartTime = events.First().Date ?? throw new Exception("Unable to determine fixture deadline based on selected events.");


            var newFixture = new Fixture(0)
            {
                Name = fixture.Name,
                Events = events,
                TiebreakerEventId = fixture.TiebreakerEventId,
                Deadline = firstEventStartTime.AddMinutes(-5)
            };

            _context.Fixtures.Add(newFixture);
            await _context.SaveChangesAsync();

            return newFixture;
        }

        public async Task<IEnumerable<Fixture>> GetFixturesAsync(string userId)
        {
            // Only get fixtures within the last 1 year
            var allFixtures = await _context.Fixtures
                .Where(f => f.Deadline > DateTime.UtcNow.AddDays(-365))
                .OrderByDescending(f => f.Deadline)
                .Include(f => f.Events)
                .ToListAsync();

            foreach (var fixture in allFixtures)
            {
                var myEntry = _context.Entries.FirstOrDefault(e => e.FixtureId == fixture.Id && e.UserId == userId);
                var numEntries = _context.Entries.Where(e => e.FixtureId == fixture.Id).Count();

                fixture.EntryCount = numEntries;
                fixture.HasSubmitted = myEntry != null;
            }

            return allFixtures;
        }

        public async Task<Fixture?> GetFixtureAsync(int id, string userId)
        {
            var fixture = await _context.Fixtures
                .Include(f => f.Entries.OrderBy(e => e.UserName))
                .Include(f => f.Events.OrderBy(e => e.Date)).ThenInclude(e => e.HomeTeam)
                .Include(f => f.Events.OrderBy(e => e.Date)).ThenInclude(e => e.AwayTeam)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (fixture != null)
            {
                fixture.HasSubmitted = fixture.Entries.Any(e => e.UserId == userId);
                fixture.EntryCount = fixture.Entries.Count;
            }

            return fixture;
        }

        public async Task<Entry> SubmitEntryAsync(Entry entry)
        {
            var existingEntry = await _context.Entries.FirstOrDefaultAsync(e => e.UserId == entry.UserId && e.FixtureId == entry.FixtureId);
            var fixture = await _context.Fixtures.FirstOrDefaultAsync(f => f.Id == entry.FixtureId) ?? throw new Exception($"Fixture {entry.FixtureId} not found.");

            if (fixture.Locked) throw new Exception("Entry submission deadline has passed.");

            if (existingEntry == null)
            {
                _context.Entries.Add(entry);
                var now = DateTime.UtcNow;
                entry.SubmittedAt = now;
                entry.UpdatedAt = now;
            }
            else
            {
                var submittedAt = existingEntry.SubmittedAt;
                _context.Entry(existingEntry).CurrentValues.SetValues(entry);
                existingEntry.SubmittedAt = submittedAt;

                foreach (var ev in existingEntry.EventEntries)
                {
                    _context.Entry(ev).CurrentValues.SetValues(entry.EventEntries.First(ee => ee.EventId == ev.EventId));
                }

                existingEntry.UpdatedAt = DateTime.UtcNow;
            }
            await _context.SaveChangesAsync();
            return entry;
        }

        public async Task<Entry?> GetEntryAsync(string userId, int fixtureId)
        {
            var entry = await _context.Entries.FirstOrDefaultAsync(e => e.UserId == userId && e.FixtureId == fixtureId);
            return entry;
        }
    }
}