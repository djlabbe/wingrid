using Microsoft.EntityFrameworkCore;
using Wingrid.Services.FixtureAPI.Data;
using Wingrid.Services.FixtureAPI.Models;
using Wingrid.Services.FixtureAPI.Models.Dto;

namespace Wingrid.Services.FixtureAPI.Services
{
    public interface IEntryService
    {
        public Task<Entry> GetAsync(int id);
        public Task<Entry> SubmitEntryAsync(Entry entry);
    }

    public class EntryService(AppDbContext context) : IEntryService
    {
        private readonly AppDbContext _context = context;

        public async Task<Entry> GetAsync(int id)
        {
            var entry = await _context.Entries.FirstAsync(e => e.Id == id);
            return entry;
        }

        public async Task<Entry> SubmitEntryAsync(Entry entry)
        {
            await Task.Delay(1);
            return entry;
        }
    }
}