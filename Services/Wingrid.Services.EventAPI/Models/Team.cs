using Wingrid.Services.EventAPI.Models.Espn;

namespace Wingrid.Services.EventAPI.Models
{
    public class Team(string id)
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

        public Team(EspnTeam espnTeam) : this(espnTeam.Id)
        {
            UpdateFrom(espnTeam);
        }

        public void UpdateFrom(EspnTeam espnTeam)
        {
            Uid = espnTeam.Uid;
            Location = espnTeam.Location;
            Name = espnTeam.Name;
            Nickname = espnTeam.Nickname;
            Abbreviation = espnTeam.Abbreviation;
            DisplayName = espnTeam.DisplayName;
            ShortDisplayName = espnTeam.ShortDisplayName;
            Color = espnTeam.Color;
            AlternateColor = espnTeam.AlternateColor;
            IsActive = espnTeam.IsActive;
            IsAllStar = espnTeam.IsAllStar;
            Logo = espnTeam.Logos?.FirstOrDefault()?.Href ?? null;
        }
    }
}