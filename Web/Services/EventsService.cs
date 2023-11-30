using Microsoft.AspNetCore.WebUtilities;
using Wingrid.Web.Models;
using static Wingrid.Web.Utility.StaticDetails;

namespace Wingrid.Web.Services
{
    public interface IEventsService
    {
        public Task<ResponseDto> GetEventsAsync(EventQueryParams query);
    }

    public class EventsService(IBaseService baseService) : IEventsService
    {
        private readonly IBaseService _baseService = baseService;

        public async Task<ResponseDto> GetEventsAsync(EventQueryParams query)
        {
            var param = new Dictionary<string, string?>();

            if (query.Season != null) param.Add("season", query.Season.ToString());
            if (query.Week != null) param.Add("week", query.Week.ToString());

            var url = QueryHelpers.AddQueryString($"{EventAPIBase}/api/events", param);

            return await _baseService.SendAsync(new RequestDto() {
                ApiType=ApiType.GET,
                Url=url
            });
        }
    }
}