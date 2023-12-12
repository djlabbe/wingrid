namespace Wingrid.Services.EventAPI.Models.Dto
{
    public class FixtureDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public List<EventDto> Events { get; set; } = [];
        public List<Entry> Entries { get; set; } = [];
        public DateTime Deadline { get; set; }
        public bool Locked { get; set; }
        public required string TiebreakerEventId { get; set; }
        public bool IsComplete { get; set; }
    }
}