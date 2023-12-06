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
        public bool Locked { get; set; }
        public DateTime? LockedAt { get; set; }
        public required string TiebreakerEventId { get; set; }
    }
}