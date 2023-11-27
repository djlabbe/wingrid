using Wingrid.Web.Models;
using static Wingrid.Web.Utility.StaticDetails;

namespace Wingrid.Web.Services
{
    public interface IFixturesService
    {
        public Task<ResponseDto> GetFixturesAsync();
    }

   public class FixturesService : IFixturesService
    {
        private readonly IBaseService _baseService;

        public FixturesService(IBaseService baseService)
        {
            _baseService = baseService;
        }

        public async Task<ResponseDto> GetFixturesAsync()
        {
            var url = $"{FixturesAPIBase}/api/events";

            return await _baseService.SendAsync(new RequestDto() {
                ApiType=ApiType.GET,
                Url=url
            });
        }
    }
}