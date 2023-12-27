namespace Wingrid.Models.Espn
{
    public class NflEventsResponse
    {
        public IEnumerable<EspnEvent>? Events { get; set; }
        public Season? Season { get; set; }
        public Week? Week { get; set; }
    }
}