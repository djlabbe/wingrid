namespace Wingrid.Services.EventAPI.Models
{
    public class Fixture(int id)
    {
        public int Id { get; set; } = id;
        public required string Name { get; set; }
        public List<Event> Events { get; set; } = [];
        public List<Entry> Entries { get; set; } = [];
        public DateTime Deadline { get; set; }
        public bool Locked => DateTime.UtcNow > Deadline;
        public required string TiebreakerEventId { get; set; }
        public bool IsComplete { get; set; }
    }
}