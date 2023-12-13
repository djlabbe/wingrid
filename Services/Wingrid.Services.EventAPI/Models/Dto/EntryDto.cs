namespace Wingrid.Services.EventAPI.Models.Dto
{
    public class EntryDto
    {
        public int Id { get; set; }
        public required string UserId { get; set; }
        public string? UserName { get; set; }
        public required int FixtureId { get; set; }
        public List<EventEntry> EventEntries { get; set; } = [];
        public int Tiebreaker { get; set; }
        public int? TiebreakerResult { get; set; }
        public DateTime SubmittedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int Score { get; set; }
        public bool Winner { get; set; }
    }

    public class EventEntryDto
    {
        public required string EventId { get; set; }
        public required bool HomeWinnerSelected { get; set; }
        public bool? HomeWinner { get; set; }
        public bool IsCorrect { get; set; }
    }
}