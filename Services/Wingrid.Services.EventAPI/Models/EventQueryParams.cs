namespace Wingrid.Services.EventAPI.Models
{
    public class EventQueryParams
    {
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
        public string? Id { get; set; }
    }
}