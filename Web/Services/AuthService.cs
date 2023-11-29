using Wingrid.Web.Models;
using static Wingrid.Web.Utility.StaticDetails;

namespace Wingrid.Web.Services
{
    public interface IAuthService
    {
        Task<ResponseDto> GetCurrentUserAsync();
        Task<ResponseDto> LoginAsync(LoginRequestDto loginRequestDto);
        Task<ResponseDto> RegisterAsync(RegistrationRequestDto registrationRequestDto);
        Task<ResponseDto> AssignRoleAsync(RegistrationRequestDto registrationRequestDto);
    }
    public class AuthService(IBaseService baseService) : IAuthService
    {
        private readonly IBaseService _baseService = baseService;

        public async  Task<ResponseDto> GetCurrentUserAsync()
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = ApiType.GET,
                Url = $"{AuthAPIBase}/api/auth"
            });
        }

        public async Task<ResponseDto> LoginAsync(LoginRequestDto loginRequestDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = ApiType.POST,
                Data = loginRequestDto,
                Url = $"{AuthAPIBase}/api/auth/login"
            }, withBearer: false);
        }

        public async Task<ResponseDto> RegisterAsync(RegistrationRequestDto registrationRequestDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = ApiType.POST,
                Data = registrationRequestDto,
                Url = $"{AuthAPIBase}/api/auth/register"
            }, withBearer: false);
        }

        public async Task<ResponseDto> AssignRoleAsync(RegistrationRequestDto registrationRequestDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = ApiType.POST,
                Data = registrationRequestDto,
                Url = $"{AuthAPIBase}/api/auth/assignrole"
            });
        }
    }
}