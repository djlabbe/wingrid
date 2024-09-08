
using Hangfire;
using Hangfire.Console;
using Hangfire.Server;
using Microsoft.EntityFrameworkCore;
using Wingrid.Data;
using Wingrid.Models;
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
                    }
                    else
                    {
                        var maxScore = fixture.Entries.OrderByDescending(e => e.Score).First().Score;
                        var tiedEntries = fixture.Entries.Where(e => e.Score == maxScore);
                        var minTb = tiedEntries.OrderBy(e => e.TiebreakerResult).First().TiebreakerResult;
                        var winningEntries = tiedEntries.Where(e => e.TiebreakerResult!.Value == minTb!.Value);
                        performContext.WriteLine($"Found {winningEntries.Count()} winning entries with score of {maxScore} and TiebreakerResult of {minTb}.");
                        foreach (var entry in winningEntries)
                        {
                            performContext.WriteLine($"Winning entry: {entry.UserId}");
                            entry.Winner = true;

                            var user = await _context.ApplicationUsers.Where(u => u.Id == entry.UserId).FirstAsync();

                            if (user.Email != null)
                            {
                                await _emailClient.SendEmailAsync(user.Email, "You won!", WinMessage(fixture), 23681);
                            }
                        }

                        // Done processing winner. Now update player statistics
                        performContext.WriteLine($"Processing user statistics...");
                        var ncaaEventIds = fixture.Events.Where(e => e.League == League.NCAA).Select(e => e.Id);
                        var nflEventIds = fixture.Events.Where(e => e.League == League.NFL).Select(e => e.Id);
                        performContext.WriteLine($"Fixture contains {ncaaEventIds.Count()} NCAA events and {nflEventIds.Count()} NFL events.");


                        foreach (var entry in fixture.Entries)
                        {
                            performContext.WriteLine($"Updating statistics for user: {entry.UserId}");

                            var userStats = await _context.UserStatistics.FirstOrDefaultAsync(us => us.UserId == entry.UserId);

                            if (userStats == null)
                            {
                                userStats = new UserStatistics(entry.UserId);
                                _context.Add(userStats);
                            }
                            userStats.Entries += 1;

                            if (entry.Winner)
                            {
                                userStats.Wins += 1;
                            }

                            userStats.TotalCollegePicks += ncaaEventIds.Count();
                            userStats.TotalProPicks += nflEventIds.Count();

                            userStats.CorrectCollegePicks += entry.EventEntries.Where(ee => ncaaEventIds.Contains(ee.EventId) && ee.IsCorrect).Count();
                            userStats.CorrectProPicks += entry.EventEntries.Where(ee => nflEventIds.Contains(ee.EventId) && ee.IsCorrect).Count();

                            userStats.TotalTieBreakerError += entry.TiebreakerResult ?? 0;
                        }
                    }
                }
                else
                {
                    performContext.WriteLine($"Fixture is not yet complete. Continuing.");
                }
            }

            var changeCount = await _context.SaveChangesAsync();
        }


        private static string WinMessage(Fixture fixture)
        {
            return $"""
                <!DOCTYPE html>
                <html lang="en">
                <head>
                <meta charset="UTF-8">
                <title>Wingrid - Winner Notification</title>
                </head>
                <body style="font-family: 'Arial', sans-serif; background-color: #f4f4f4; color: #333;">

                <div style="margin: 0 auto; padding: 20px; background-color: #fff; border-radius: 10px; box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);">
                    <img src="https://wingrid.io/logo.png" alt="Wingrid Logo" style="width: 150px; margin-bottom: 20px;">

                    <h2 style="color: #16a34a;">Congratulations!</h2>

                    <p>You are a winner on Wingrid! Your expertise in predicting football outcomes has paid off, and we're excited to announce that you've won: "{fixture.Name}"</p>

                    <p>Please log in to your <a href="https://wingrid.io">Wingrid</a> account to view detailed results and enter new contests.</p>

                    <p>If you have any questions or concerns, feel free to contact our support team at <a href="mailto:admin@wingrid.io">admin@wingrid.io</a></p>

                    <p>Thank you for participating in Wingrid!</p>

                    <p>Best regards,<br> The Wingrid Team</p>
                    <div style="text-align: center"><small><a href="<%asm_group_unsubscribe_raw_url%>">Unsubscribe</a> <a href="<%asm_preferences_raw_url%>">Preferences</a></small><div>
                </div>
                </body>
                </html>
            """;
        }
    }


}