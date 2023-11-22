
namespace Wingrid.Collector.Models.Espn
{
    public class Status
    {
        // public int? Clock { get; set; }
        public string? DisplayClock { get; set; }
        public int? Period { get; set; }

        public StatusType? Type { get; set; }

        public class StatusType
        {
            public string? Id { get; set; }
            public string? Name { get; set; }
            public string? State { get; set; }
            public bool Completed { get; set; }
            public string? Description { get; set; }
            public string? Detail { get; set; }
            public string? ShortDetail { get; set; }
        }
    }
}