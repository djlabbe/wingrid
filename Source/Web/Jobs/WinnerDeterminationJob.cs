
using Hangfire;
using Hangfire.Console;
using Hangfire.Server;
using Microsoft.EntityFrameworkCore;
using Wingrid.Data;
using Wingrid.Models;
using Wingrid.Models.Dto;
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

                        var user = await _context.ApplicationUsers.Where(u => u.Id == entry.UserId).FirstAsync();

                        if (user.Email != null)
                        {
                            await _emailClient.SendEmailAsync(user.Email, "You won!", winMessage(fixture), 23681);
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


        private string winMessage(Fixture fixture)
        {
            return $"""
                <!DOCTYPE html>
                <html lang="en">
                <head>
                <meta charset="UTF-8">
                <meta name="viewport" content="width=device-width, initial-scale=1.0">
                <title>Wingrid - Winner Notification</title>
                </head>
                <body style="font-family: 'Arial', sans-serif; background-color: #f4f4f4; color: #333;">

                <div style="max-width: 250px; margin: 0 auto; padding: 20px; background-color: #fff; border-radius: 10px; box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);">
                    <img src="https://wingrid.io/logo.png" alt="Wingrid Logo" style="width: 150px; margin-bottom: 20px;">

                    <h2 style="color: rgb(22 163 74);">Congratulations!</h2>

                    <p>You are a winner on Wingrid! Your expertise in predicting football outcomes has paid off, and we're excited to announce that you've won competition: "{fixture.Name}"</p>

                    <p>Please log in to your Wingrid account to view detailed results and enter new contests.</p>

                    <a href="https://wingrid.io" style="display: inline-block; padding: 10px 20px; background-color: rgb(22 163 74); color: #fff; text-decoration: none; border-radius: 5px;">Wingrid</a>

                    <p>If you have any questions or concerns, feel free to contact our support team at admin@wingridapp.io.</p>

                    <p>Thank you for participating in Wingrid!</p>

                    <p>Best regards,<br> The Wingrid Team</p>
                </div>
                <div><small><a href=\"<%asm_group_unsubscribe_raw_url%>\">Unsubscribe</a> <a href=\"<%asm_preferences_raw_url%>\">Preferences</a></small><div>
                </body>
                </html>
            """;
        }
    }


}