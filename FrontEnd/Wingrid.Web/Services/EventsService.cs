using Microsoft.AspNetCore.WebUtilities;
using Wingrid.Web.Models;
using static Wingrid.Web.Utility.StaticDetails;

namespace Wingrid.Web.Services
{
    public interface IEventsService
    {
        public Task<ResponseDto> GetEventsAsync(int? season, int? week);
    }

    public class EventsService : IEventsService
    {
        private readonly IBaseService _baseService;

        public EventsService(IBaseService baseService)
        {
            _baseService = baseService;
        }

        public async Task<ResponseDto> GetEventsAsync(int? season, int? week)
        {
            var param = new Dictionary<string, string?>();

            if (season != null) param.Add("season", season.ToString());
            if (week != null) param.Add("week", week.ToString());

            var url = QueryHelpers.AddQueryString($"{CollectorAPIBase}/api/events", param);

            return await _baseService.SendAsync(new RequestDto() {
                ApiType=ApiType.GET,
                Url=url
            });
        }
    }
}