using System.ComponentModel.DataAnnotations.Schema;

namespace Wingrid.Services.FixtureAPI.Models
{
    public class Fixture
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public List<string> EventIds { get; set; } = [];
        [NotMapped]
        public List<EventDto> Events { get; set; } = [];
        public List<Entry> Entries { get; set; } = [];
        public DateTime Deadline { get; set; }
        public bool Locked => DateTime.UtcNow > Deadline;
        public required string TiebreakerEventId { get; set; }
    }
}