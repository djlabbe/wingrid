using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Wingrid.Services.FixtureAPI.Data;
using Wingrid.Services.FixtureAPI.Models;

namespace Wingrid.Services.FixtureAPI.Services
{
    public interface IScoreService
    {
        Task ProcessFinalScore(EventDto eventDto);
    }

    // Cannot use the Scoped DB Context in the Singleton AzureBusConsumer, so this service uses a singleton DB Context.
    public class ScoreService(DbContextOptions<AppDbContext> dbOptions) : IScoreService
    {
        private DbContextOptions<AppDbContext> _dbOptions = dbOptions;

        public async Task ProcessFinalScore(EventDto eventDto)
        {
            await using var _db = new AppDbContext(_dbOptions);
            var entries = _db.Entries.Where(e => e.EventEntries.Any(ee => ee.EventId == eventDto.Id));

            foreach (var entry in entries)
            {
                var eeToUpdate = entry.EventEntries.FirstOrDefault(ee => ee.EventId == eventDto.Id);
                if (eeToUpdate != null) eeToUpdate.HomeWinner = eventDto.HomeWinner;
            }

            await _db.SaveChangesAsync();
        }
    }
}