namespace Wingrid.Web.Models
{
    public class FixtureDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public IEnumerable<string> EventIds { get; set; } = [];
        public bool Locked { get; set; }
        public DateTime? LockedAt { get; set; }
        public required string TiebreakerEventId { get; set; }

    }
}