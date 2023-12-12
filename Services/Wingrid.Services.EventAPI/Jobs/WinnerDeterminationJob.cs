
using Hangfire;
using Hangfire.Console;
using Hangfire.Server;
using Microsoft.EntityFrameworkCore;
using Wingrid.Services.EventAPI.Data;

namespace Wingrid.Services.EventAPI.Jobs
{
    [AutomaticRetry(Attempts = 0)]
    public class WinnerDeterminationJob(AppDbContext context) : IBatchJob
    {
        private readonly AppDbContext _context = context;

        public static string JobId => "WinnerDetermination";

        public async Task ExecuteAsync(PerformContext? performContext)
        {
            var fixtures = await _context.Fixtures
                .Include(f => f.Events)
                .Include(f => f.Entries)
                .Where(f => !f.IsComplete)
                .ToListAsync();

            performContext.WriteLine($"Found {fixtures.Count} open fixtures.");

            foreach (var fixture in fixtures)
            {
                performContext.WriteLine($"Processing fixture {fixture.Id}.");

                if (fixture.Events.All(e => e.StatusCompleted == true))
                {
                    fixture.IsComplete = true;
                    if (fixture.Entries.Count == 0)
                    {
                        performContext.WriteLine($"Fixture has no entries. Marked complete and continuing.");
                    };

                    var maxScore = fixture.Entries.OrderByDescending(e => e.Score).First().Score;
                    var tiedEntries = fixture.Entries.Where(e => e.Score == maxScore);
                    var minTb = tiedEntries.OrderBy(e => e.TiebreakerResult).First().TiebreakerResult;
                    var winningEntries = tiedEntries.Where(e => e.TiebreakerResult!.Value == minTb!.Value);
                    performContext.WriteLine($"Found {winningEntries.Count()} winning entries with score of {maxScore} and TiebreakerResult of {minTb}.");
                    foreach (var entry in winningEntries)
                    {
                        performContext.WriteLine($"Winning entry: {entry.Id}");
                        entry.Winner = true;
                    }
                }
                else
                {
                    performContext.WriteLine($"Fixture is not yet complete. Continuing.");
                }
            }

            var changeCount = await _context.SaveChangesAsync();
        }
    }
}