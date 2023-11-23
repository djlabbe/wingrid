
using Wingrid.Services.Collector.Models.Espn;

namespace Wingrid.Services.Collector.Models
{
    public class Event
    {
        // Event
        public string Id { get; set; }
        public string? Uid { get; set; }
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

        public Event(string id)
        {
            Id = id;
        }

        public Event(EspnEvent espnEvent) : this(espnEvent.Id)
        {
            UpdateFrom(espnEvent);
        }

        public void UpdateFrom(EspnEvent espn)
        {
            var season = espn.Season;
            var week = espn.Week;
            var competition = espn.Competitions?.FirstOrDefault();
            var homeCompetitor = competition?.Competitors?.FirstOrDefault(c => c.HomeAway == "home");
            var awayCompetitor = competition?.Competitors?.FirstOrDefault(c => c.HomeAway == "away");
            var homeTeamId = homeCompetitor?.Team?.Id;
            var awayTeamId = awayCompetitor?.Team?.Id;
            var eventStatus = espn.Status;

            if (Id != espn.Id) throw new Exception("Event ids must match.");

            Uid = espn.Uid;
            Date = espn.Date;
            Name = espn.Name;
            ShortName = espn.ShortName;
            SeasonType = season?.Type;
            Season = season?.Year;
            SeasonSlug = season?.Slug;
            Week = week?.Number;
            Attendance = competition?.Attendance;
            NeutralSite = competition?.NeutralSite;
            TimeValid = competition?.TimeValid;
            ConferenceCompetition = competition?.ConferenceCompetition;
            PlayByPlayAvailable = competition?.PlayByPlayAvailable;
            Recent = competition?.Recent;
            HomeTeamId = homeTeamId;
            HomeWinner = homeCompetitor?.Winner;
            HomeScore = homeCompetitor?.Score;
            AwayTeamId = awayTeamId;
            AwayWinner = awayCompetitor?.Winner;
            AwayScore = awayCompetitor?.Score;
            // Clock = eventStatus?.Clock;
            DisplayClock = eventStatus?.DisplayClock;
            Period = eventStatus?.Period;
            StatusId = eventStatus?.Type?.Id;
            StatusName = eventStatus?.Type?.Name;
            StatusState = eventStatus?.Type?.State;
            StatusCompleted = eventStatus?.Type?.Completed;
            StatusDescription = eventStatus?.Type?.Description;
            StatusDetail = eventStatus?.Type?.Detail;
            StatusShortDetail = eventStatus?.Type?.ShortDetail;
        }
    }
}