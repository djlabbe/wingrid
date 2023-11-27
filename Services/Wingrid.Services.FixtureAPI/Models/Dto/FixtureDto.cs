namespace Wingrid.Services.FixtureAPI.Models.Dto
{
    public class FixtureDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public List<string> EventIds { get; set; } = [];
        public List<EventDto> Events { get; set; } = [];
        public bool Locked { get; set; }
        public DateTime LockedAt { get; set; }
        public List<Entry> Entries { get; set; } = [];
    }
}