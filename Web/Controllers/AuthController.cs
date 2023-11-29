using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Wingrid.Web.Models;
using Wingrid.Web.Services;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;

namespace Wingrid.Web.Controllers
{
    [Route("api/auth")]
    public class AuthController(IAuthService authService, ITokenProvider tokenProvider) : BaseController<AuthController>
    {
        private readonly IAuthService _authService = authService;
        private readonly ITokenProvider _tokenProvider = tokenProvider;

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetCurrentUser()
        {
            return await ExecuteActionAsync(async () =>
            {
                ResponseDto responseDto = await _authService.GetCurrentUserAsync();
                if (responseDto.IsSuccess)
                {
                    var resultStr = Convert.ToString(responseDto.Result) ?? throw new Exception("Error deserializing Auth API response.");
                    LoginResponseDto loginResponseDto = JsonConvert.DeserializeObject<LoginResponseDto>(resultStr) ?? throw new Exception("Error deserializing Auth API response.");
                    if (loginResponseDto.Token != null)
                    {
                        return Ok(loginResponseDto);
                    }
                    else
                    {
                        throw new Exception("JWT token error.");
                    }
                }
                {
                    throw new Exception(responseDto.Message);
                }
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestDto loginRequestDto)
        {
            return await ExecuteActionAsync(async () =>
            {
                ResponseDto responseDto = await _authService.LoginAsync(loginRequestDto);

                if (responseDto.IsSuccess && responseDto.Result != null)
                {
                    var resultStr = Convert.ToString(responseDto.Result) ?? throw new Exception("Error deserializing Auth API response.");
                    LoginResponseDto loginResponseDto = JsonConvert.DeserializeObject<LoginResponseDto>(resultStr) ?? throw new Exception("Error deserializing Auth API response.");
                    if (loginResponseDto.Token != null)
                    {
                        await SignInUser(loginResponseDto);
                        _tokenProvider.SetToken(loginResponseDto.Token);
                        return Ok(loginResponseDto);
                    }
                    else
                    {
                        throw new Exception("JWT token error.");
                    }
                }
                else
                {
                    throw new Exception(responseDto.Message);
                }
            });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegistrationRequestDto registrationRequestDto)
        {
            return await ExecuteActionAsync(async () =>
            {
                ResponseDto registrationResponseDto = await _authService.RegisterAsync(registrationRequestDto);

                if (registrationResponseDto.IsSuccess)
                {
                    if (string.IsNullOrEmpty(registrationRequestDto.Role))
                    {
                        registrationRequestDto.Role = "User";
                    }
                    ResponseDto roleResponseDto = await _authService.AssignRoleAsync(registrationRequestDto);
                    if (roleResponseDto.IsSuccess)
                    {
                        return Ok(roleResponseDto.Result);
                    }
                    throw new Exception(roleResponseDto.Message);
                }
                else
                {
                    throw new Exception(registrationResponseDto.Message);
                }
            });
        }

        [HttpGet("logout")]
        public async Task<IActionResult> Logout()
        {
            return await ExecuteActionAsync(async () =>
            {
                await HttpContext.SignOutAsync();
                _tokenProvider.ClearToken();
                return Ok();
            });
        }

        private async Task SignInUser(LoginResponseDto model)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(model.Token);
            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Email, jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Email)?.Value ?? ""));
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Sub, jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Sub)?.Value ?? ""));
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Name, jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Name)?.Value ?? ""));
            identity.AddClaim(new Claim(ClaimTypes.Name, jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Email)?.Value ?? ""));
            identity.AddClaim(new Claim(ClaimTypes.Role, jwt.Claims.FirstOrDefault(u => u.Type == "role")?.Value ?? ""));

            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }
    }
}