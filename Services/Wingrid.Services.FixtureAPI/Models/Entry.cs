using Microsoft.EntityFrameworkCore;

namespace Wingrid.Services.FixtureAPI.Models
{
    [PrimaryKey(nameof(UserId), nameof(FixtureId))]
    public class Entry
    {
        public int Id { get; set; }
        public required string UserId { get; set; }
        public required int FixtureId { get; set; }
        public required Fixture Fixture { get; set; }
        public List<EventEntry> EventEntries { get; set; } = [];
        public int Tiebreaker { get; set; }
        public int Score => EventEntries.Where(ee => ee.IsCorrect).Count();
        public DateTime SubmittedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class EventEntry
    {
        public required string EventId { get; set; }
        public required bool HomeWinnerSelected { get; set; }
        public bool? HomeWinner { get; set; }
        public bool IsCorrect => HomeWinnerSelected == HomeWinner;
    }
}