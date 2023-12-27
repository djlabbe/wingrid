namespace Wingrid.Models.Dto
{
    public class CreateFixtureDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public List<string> EventIds { get; set; } = [];
        public required string TiebreakerEventId { get; set; }
    }
}