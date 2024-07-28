namespace Wingrid.Models
{
    public class EventQueryParams
    {
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
        public int? TimezoneOffset { get; set; }
        public string? Id { get; set; }
    }
}