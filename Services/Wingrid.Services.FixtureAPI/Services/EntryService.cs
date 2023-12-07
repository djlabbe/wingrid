using Microsoft.EntityFrameworkCore;
using Wingrid.Services.FixtureAPI.Data;
using Wingrid.Services.FixtureAPI.Models;

namespace Wingrid.Services.FixtureAPI.Services
{
    public interface IEntryService
    {
        public Task<Entry?> GetAsync(string userId, int fixtureId);
        public Task<Entry> SubmitEntryAsync(Entry entry);
    }

    public class EntryService(AppDbContext context) : IEntryService
    {
        private readonly AppDbContext _context = context;

        public async Task<Entry?> GetAsync(string userId, int fixtureId)
        {
            var entry = await _context.Entries.FirstOrDefaultAsync(e => e.UserId == userId && e.FixtureId == fixtureId);
            return entry;
        }

        public async Task<Entry> SubmitEntryAsync(Entry entry)
        {
            var existingEntry = await _context.Entries.FirstOrDefaultAsync(e => e.UserId == entry.UserId && e.FixtureId == entry.FixtureId);
            var fixture = await _context.Fixtures.FirstOrDefaultAsync(f => f.Id == entry.FixtureId) ?? throw new Exception($"Fixture {entry.FixtureId} not found.");

            if (fixture.Locked) throw new Exception("Entry submission deadline has passed.");

            if (existingEntry == null)
            {
                _context.Entries.Add(entry);
                entry.SubmittedAt = DateTime.UtcNow;
            }
            else
            {
                _context.Entry(existingEntry).CurrentValues.SetValues(entry);

                foreach (var ev in existingEntry.EventEntries)
                {
                    _context.Entry(ev).CurrentValues.SetValues(entry.EventEntries.First(ee => ee.EventId == ev.EventId));
                }

                existingEntry.UpdatedAt = DateTime.UtcNow;
            }
            await _context.SaveChangesAsync();
            return entry;
        }
    }
}