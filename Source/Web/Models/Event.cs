
using Wingrid.Models.Espn;

namespace Wingrid.Models
{
    public class Event(League league, string id)
    {
        // League
        public League League { get; set; } = league;

        // Event
        public string Id { get; set; } = id;
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
        public int? HomeTeamId { get; set; }
        public Team? HomeTeam { get; set; }
        public int? HomeScore { get; set; }

        // Away Competitor
        public bool? AwayWinner { get; set; }
        public int? AwayTeamId { get; set; }
        public Team? AwayTeam { get; set; }
        public int? AwayScore { get; set; }

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
        public List<Fixture> Fixtures { get; set; } = [];

        public Event(League league, EspnEvent espnEvent) : this(league, espnEvent.Id)
        {
            UpdateFrom(league, espnEvent);
        }


        // Updates all properties, except Teams (foreign keys)
        public void UpdateFrom(League league, EspnEvent espn)
        {
            League = league;

            var season = espn.Season;
            var week = espn.Week;
            var competition = espn.Competitions!.First();
            var homeCompetitor = competition.Competitors!.First(c => c.HomeAway == "home");
            var awayCompetitor = competition.Competitors!.First(c => c.HomeAway == "away");
            var homeTeamId = homeCompetitor.Team!.Id;
            var awayTeamId = awayCompetitor.Team!.Id;
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
            HomeWinner = homeCompetitor.Winner;
            HomeScore = homeCompetitor.Score == null ? null : int.Parse(homeCompetitor.Score);
            AwayWinner = awayCompetitor.Winner;
            AwayScore = awayCompetitor.Score == null ? null : int.Parse(awayCompetitor.Score);
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