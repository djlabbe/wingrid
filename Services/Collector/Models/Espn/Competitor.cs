namespace Wingrid.Services.Collector.Models.Espn
{
    public class Competitor
    {
        public Competitor(string id) {
            Id = id;
        }

        public string Id { get; set; }
        public string? Uid { get; set; }
        public string? Type { get; set; }
        public int? Order { get; set; }
        public string? HomeAway { get; set; }
        public bool? Winner { get; set; }
        public Team? Team { get; set; }
        public string? Score { get; set; }
    }
}