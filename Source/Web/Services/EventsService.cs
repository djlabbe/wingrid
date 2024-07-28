using Microsoft.EntityFrameworkCore;
using Wingrid.Data;
using Wingrid.Models;
using Wingrid.Models.Espn;

namespace Wingrid.Services
{
    public interface IEventsService
    {
        public Task<IEnumerable<Event>> GetEventsAsync(EventQueryParams eventQueryParams);
        public Task<IEnumerable<Event>> GetEventsAsync(IEnumerable<string> ids);
        public Task<Event?> GetEventAsync(string id);
        public Task<Event> AddOrUpdateEventAsync(League league, EspnEvent espnEvent);
        public Task<int> SaveChangesAsync();
    }

    public class EventsService(AppDbContext context) : IEventsService
    {
        private readonly AppDbContext _context = context;

        public async Task<IEnumerable<Event>> GetEventsAsync(EventQueryParams eventQueryParams)
        {
            var offset = eventQueryParams.TimezoneOffset;
            var start = eventQueryParams.Start;
            var end = eventQueryParams.End;

            var ids = eventQueryParams.Id?.Split(",") ?? [];

            List<Event> events;
            if (start == null && end == null)
            {
                events = await _context.Events.Include(e => e.HomeTeam).Include(e => e.AwayTeam).OrderBy(e => e.Date).ThenBy(e => e.AwayTeam == null ? "" : e.AwayTeam.DisplayName).ToListAsync();
            }
            else if (start != null && end == null || start == null && end != null)
            {
                throw new Exception("Must provide both start and end");
            }
            else
            {
                if (start != null && offset != null)
                    start = start.Value.AddMinutes((double)offset);

                if (end != null && offset != null)
                    end = end.Value.AddMinutes((double)offset);

                events = await _context.Events.Where(e => e.Date >= start && e.Date <= end)
                    .Include(e => e.HomeTeam)
                    .Include(e => e.AwayTeam)
                    .OrderBy(e => e.Date)
                    .ThenBy(e => e.AwayTeam == null ? "" : e.AwayTeam.DisplayName)
                    .ToListAsync();
            }

            if (ids.Length != 0)
            {
                events = events.Where(e => ids.Contains(e.Id)).ToList();
            }

            return events;
        }

        public async Task<IEnumerable<Event>> GetEventsAsync(IEnumerable<string> ids)
        {
            var events = await _context.Events.Where(e => ids.Contains(e.Id)).Include(e => e.HomeTeam).Include(e => e.AwayTeam).OrderBy(e => e.Date).ToListAsync();
            return events;
        }

        public Task<Event?> GetEventAsync(string id)
        {
            var evnt = _context.Events.FirstOrDefaultAsync(e => e.Id == id);
            return evnt;
        }

        public async Task<Event> AddOrUpdateEventAsync(League league, EspnEvent espnEvent)
        {
            var evnt = await _context.Events.FirstOrDefaultAsync(e => e.Id == espnEvent.Id);

            if (evnt == null)
            {
                evnt = new Event(espnEvent);

                var homeTeamEspnId = espnEvent.Competitions?.FirstOrDefault()?.Competitors?.FirstOrDefault(c => c.HomeAway == "home")?.Team?.Id;
                var awayTeamEspnId = espnEvent.Competitions?.FirstOrDefault()?.Competitors?.FirstOrDefault(c => c.HomeAway == "away")?.Team?.Id;

                evnt.HomeTeamId = _context.Teams.FirstOrDefault(t => t.League == league && t.EspnId == homeTeamEspnId)?.Id;
                evnt.AwayTeamId = _context.Teams.FirstOrDefault(t => t.League == league && t.EspnId == awayTeamEspnId)?.Id;

                _context.Add(evnt);
            }
            else
            {
                var prevStatus = evnt.StatusCompleted;

                evnt.UpdateFrom(espnEvent);

                var homeTeamEspnId = espnEvent.Competitions?.FirstOrDefault()?.Competitors?.FirstOrDefault(c => c.HomeAway == "home")?.Team?.Id;
                var awayTeamEspnId = espnEvent.Competitions?.FirstOrDefault()?.Competitors?.FirstOrDefault(c => c.HomeAway == "away")?.Team?.Id;

                evnt.HomeTeamId = _context.Teams.FirstOrDefault(t => t.League == league && t.EspnId == homeTeamEspnId)?.Id;
                evnt.AwayTeamId = _context.Teams.FirstOrDefault(t => t.League == league && t.EspnId == awayTeamEspnId)?.Id;



                if (prevStatus != true && evnt.StatusCompleted == true) // Event is now completed
                {
                    // Score all Entries for this Event
                    var entries = _context.Entries.Where(e => e.EventEntries.Any(ee => ee.EventId == evnt.Id));
                    foreach (var entry in entries)
                    {
                        var eeToUpdate = entry.EventEntries.FirstOrDefault(ee => ee.EventId == evnt.Id);
                        if (eeToUpdate != null)
                        {
                            eeToUpdate.HomeWinner = evnt.HomeWinner;
                            eeToUpdate.AwayWinner = evnt.AwayWinner;
                        }
                    }

                    // Find all Fixtures where this event is the tiebreaker and update entry tiebreaker results
                    var fixturesWithTiebreak = _context.Fixtures.Include(f => f.Entries).Where(f => f.TiebreakerEventId == evnt.Id);

                    var totalScore = evnt.HomeScore + evnt.AwayScore;
                    if (totalScore != null)
                    {
                        foreach (var fixture in fixturesWithTiebreak)
                        {
                            foreach (var e in fixture.Entries)
                            {
                                e.TiebreakerResult = Math.Abs(e.Tiebreaker - totalScore.Value);
                            }
                        }
                    }

                    await _context.SaveChangesAsync();
                }
            }

            return evnt;
        }


        public Task<int> SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }
    }
}