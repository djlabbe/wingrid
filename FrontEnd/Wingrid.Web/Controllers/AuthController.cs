using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Wingrid.Web.Models;
using Wingrid.Web.Services;
using Wingrid.Web.Utility;
using System.IdentityModel.Tokens.Jwt;
using System.Net;

namespace Wingrid.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly ITokenProvider _tokenProvider;

        public AuthController(IAuthService authService, ITokenProvider tokenProvider)
        {
            _authService = authService;
            _tokenProvider = tokenProvider;
        }

        [HttpGet]
        public IActionResult Login()
        {
            LoginRequestDto loginRequestDto = new ();
            return View(loginRequestDto);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestDto loginRequestDto)
        {
            ResponseDto responseDto = await _authService.LoginAsync(loginRequestDto);

            if (responseDto.IsSuccess) {
                LoginResponseDto? loginResponseDto = JsonConvert.DeserializeObject<LoginResponseDto>(Convert.ToString(responseDto.Result) ?? "") ;
                if (loginResponseDto != null && loginResponseDto.Token != null)
                {
                    await SignInUser(loginResponseDto);
                    _tokenProvider.SetToken(loginResponseDto.Token);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                     TempData["error"] = "Error processing login response";
                    return View(loginRequestDto);
                }
            }       
            else
            {
                TempData["error"] = responseDto.Message;
                return View(loginRequestDto);
            }

        }

        [HttpGet]
        public IActionResult Register()
        {
            var roleList = new List<SelectListItem>()
            {
                new() {Text=StaticDetails.RoleAdmin, Value=StaticDetails.RoleAdmin},
                new() {Text=StaticDetails.RoleUser, Value=StaticDetails.RoleUser}
            };

            ViewBag.RoleList = roleList;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegistrationRequestDto registrationRequestDto)
        {
            ResponseDto registrationResponseDto = await _authService.RegisterAsync(registrationRequestDto);

            if ( registrationResponseDto.IsSuccess) {
                if (string.IsNullOrEmpty(registrationRequestDto.Role)) {
                    registrationRequestDto.Role = StaticDetails.RoleUser;
                }
                var roleResponseDto = await _authService.AssignRoleAsync(registrationRequestDto);
                if (roleResponseDto != null && roleResponseDto.IsSuccess) {
                    TempData["success"] = "Registration Successful";
                    return RedirectToAction(nameof(Login));
                }
            }
            else
            {
                TempData["error"] = registrationResponseDto.Message;
            }

            var roleList = new List<SelectListItem>()
            {
                new() {Text=StaticDetails.RoleAdmin, Value=StaticDetails.RoleAdmin},
                new() {Text=StaticDetails.RoleUser, Value=StaticDetails.RoleUser}
            };

            ViewBag.RoleList = roleList;
            return View(registrationRequestDto);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            _tokenProvider.ClearToken();
            return RedirectToAction("Index", "Home");
        }

        private async Task SignInUser(LoginResponseDto model)
        {
            var handler =  new JwtSecurityTokenHandler();
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