namespace Wingrid.Services.Collector.Models.Dto
{
    public class EventDto
    {
        // Event
        public string? Id { get; set; }
        public DateTime? Date { get; set; }
        public string? Name { get; set; }
        public string? ShortName { get; set; }

        // Season
        public int? SeasonType { get; set; }
        public int? Season { get; set; }
        public string? SeasonSlug { get; set; }

        // Week
        public int? Week { get; set; }

        // Competition
        public int? Attendance { get; set; }
        public bool? NeutralSite { get; set; }
        public bool? TimeValid { get; set; }
        public bool? ConferenceCompetition { get; set; }
        public bool? PlayByPlayAvailable { get; set; }
        public bool? Recent { get; set; }

        // Home Competitor
        public bool? HomeWinner { get; set; }
        public string? HomeTeamId {get; set;}
        public Team? HomeTeam { get; set; }
        public string? HomeScore { get; set; }

        // Away Competitor
        public bool? AwayWinner { get; set; }
        public string? AwayTeamId {get; set;}
        public Team? AwayTeam { get; set; }
        public string? AwayScore { get; set; }

        // Status
        // public int? Clock { get; set; }
        public string? DisplayClock { get; set; }
        public int? Period { get; set; }

        // Status Type
        public string? StatusId { get; set; }
        public string? StatusName { get; set; }
        public string? StatusState { get; set; }
        public bool? StatusCompleted { get; set; }
        public string? StatusDescription { get; set; }
        public string? StatusDetail { get; set; }
        public string? StatusShortDetail { get; set; }
    }
}