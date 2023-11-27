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
            var entries = await _db.Entries.SelectMany(
                e => e.EventEntries.Where(ee => ee.EventId == eventDto.Id)
            ).ToListAsync();

            foreach (var e in entries)
            {
                e.HomeWinner = eventDto.HomeWinner;
            }
            
            await _db.SaveChangesAsync();
        }
    }
}