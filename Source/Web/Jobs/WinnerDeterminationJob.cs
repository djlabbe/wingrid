
using Hangfire;
using Hangfire.Console;
using Hangfire.Server;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Wingrid.Data;
using Wingrid.Services;

namespace Wingrid.Jobs
{
    [AutomaticRetry(Attempts = 0)]
    public class WinnerDeterminationJob(AppDbContext context, IEmailService emailClient) : IBatchJob
    {
        private readonly AppDbContext _context = context;
        private readonly IEmailService _emailClient = emailClient;

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
                        await _emailClient.SendEmailAsync("dougjlabbe@gmail.com", "You won!", "You won the grid!", 23681);
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