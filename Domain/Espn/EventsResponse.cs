namespace Domain.Espn
{
    public class EventsResponse
    {
        public IEnumerable<EspnEvent>? Events { get; set; }
        public Season? Season { get; set; }
        public Week? Week { get; set; }
    }
}