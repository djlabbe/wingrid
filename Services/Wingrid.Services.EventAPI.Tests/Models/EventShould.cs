using Wingrid.Services.EventAPI.Models;

namespace Wingrid.EventAPI.Tests.Models
{
    public class EventShould
    {
        [Fact]
        public void EventId()
        {
            var e = new Event("99999");
            Assert.Equal("99999", e.Id);
        }
    }
}