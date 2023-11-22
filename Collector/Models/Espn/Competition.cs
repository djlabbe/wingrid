namespace Wingrid.Collector.Models.Espn
{
    public class Competition
    {
        public Competition(string id) {
            Id = id;
        }

        public string Id { get; set; }
        public string? Uid { get; set; }
        public DateTime? Date { get; set; }
        public int? Attendance { get; set; }
        public bool? NeutralSite { get; set; }
        public bool? TimeValid { get; set; }
        public bool? ConferenceCompetition { get; set; }
        public bool? PlayByPlayAvailable { get; set; }
        public bool? Recent { get; set; }
        
        public IEnumerable<Competitor>? Competitors { get; set; }
    }
}