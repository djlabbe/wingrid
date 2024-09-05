using Microsoft.EntityFrameworkCore;

namespace Wingrid.Models
{
    [PrimaryKey(nameof(UserId), nameof(FixtureId))]
    public class Entry
    {
        public required string UserId { get; set; }
        public required string UserName { get; set; }
        public required int FixtureId { get; set; }
        public List<EventEntry> EventEntries { get; set; } = [];
        public int Tiebreaker { get; set; }
        public int? TiebreakerResult { get; set; }
        public DateTime SubmittedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int Score => EventEntries.Where(ee => ee.IsCorrect).Count();
        public bool Winner { get; set; }
    }

    public class EventEntry
    {
        public required string EventId { get; set; }
        public required bool HomeWinnerSelected { get; set; }
        public bool? HomeWinner { get; set; }
        public bool? AwayWinner { get; set; }
        public bool IsCorrect => (HomeWinnerSelected && HomeWinner == true) || (!HomeWinnerSelected && AwayWinner == true); // Handle Ties
    }
}