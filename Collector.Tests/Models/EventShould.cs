using Wingrid.Collector.Models;

namespace Collector.Tests.Models
{
    public class PrimeService_IsPrimeShould
    {
        [Fact]
        public void EventId()
        {
            var e = new Event("99999");
            Assert.Equal("99999", e.Id);
        }
    }
}