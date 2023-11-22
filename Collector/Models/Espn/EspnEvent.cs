namespace Wingrid.Collector.Models.Espn
{
    public class EspnEvent
    {
        public string Id { get; set; }
        public string? Uid { get; set; }
        public DateTime? Date { get; set; }
        public string? Name { get; set; }
        public string? ShortName { get; set; }
        public Season? Season { get; set; }
        public Week? Week { get; set; }
        public Status? Status { get; set; }
        public IEnumerable<Competition>? Competitions {get; set;}
 
        public EspnEvent(string id)
        {
            Id = id;
        }
    }
}