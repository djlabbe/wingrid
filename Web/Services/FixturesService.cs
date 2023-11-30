using Wingrid.Web.Models;
using static Wingrid.Web.Utility.StaticDetails;

namespace Wingrid.Web.Services
{
    public interface IFixturesService
    {
        public Task<ResponseDto> GetFixturesAsync();
        public Task<ResponseDto> CreateFixtureAsync(FixtureDto fixture);
    }

    public class FixturesService(IBaseService baseService) : IFixturesService
    {
        private readonly IBaseService _baseService = baseService;

        public async Task<ResponseDto> GetFixturesAsync()
        {
            var url = $"{FixturesAPIBase}/api/fixtures";

            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = ApiType.GET,
                Url = url
            });
        }

        public async Task<ResponseDto> CreateFixtureAsync(FixtureDto fixture)
        {
            var url = $"{FixturesAPIBase}/api/fixtures";

            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = ApiType.POST,
                Url = url,
                Data = fixture
            });
        }
    }
}