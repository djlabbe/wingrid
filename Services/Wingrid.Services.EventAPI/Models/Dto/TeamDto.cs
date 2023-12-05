namespace Wingrid.Services.EventAPI.Models.Dto
{
    public class TeamDto(string id)
    {
        public string Id { get; set; } = id;
        public string? Uid { get; set; }
        public string? Location { get; set; }
        public string? Name { get; set; }
        public string? Nickname { get; set; }
        public string? Abbreviation { get; set; }
        public string? DisplayName { get; set; }
        public string? ShortDisplayName { get; set; }
        public string? Color { get; set; }
        public string? AlternateColor { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsAllStar { get; set; }
        public string? Logo { get; set; }

    }
}