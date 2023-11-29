
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wingrid.Services.Auth.Models.Dto;
using Wingrid.Services.Auth.Services;

namespace Wingrid.Services.Auth.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        private readonly IAuthService _authService = authService;

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetCurrentUser()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);

            var loginResponse = await _authService.GetCurrentUser(userEmail);
             if (loginResponse.User == null)
            {
                var res = new ResponseDto()
                {
                    IsSuccess = false,
                    Message = "Error logging in. Please try logging in again.",
                };
                return BadRequest(res);
            }

            return Ok(new ResponseDto() { Result = loginResponse });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequestDto model)
        {
            var errorMessage = await _authService.Register(model);
            if (!string.IsNullOrEmpty(errorMessage))
            {
                var res = new ResponseDto()
                {
                    IsSuccess = false,
                    Message = errorMessage,
                };

                return BadRequest(res);
            }
            return Ok(new ResponseDto());
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto model)
        {
            var loginResponse = await _authService.Login(model);
            if (loginResponse.User == null)
            {
                var res = new ResponseDto()
                {
                    IsSuccess = false,
                    Message = "Username or password is incorrect",
                };
                return BadRequest(res);
            }

            return Ok(new ResponseDto() { Result = loginResponse });
        }

        [HttpPost("AssignRole")]
        public async Task<IActionResult> AssignRole([FromBody] RegistrationRequestDto model)
        {
            if (string.IsNullOrWhiteSpace(model.Email))
            {
                var res = new ResponseDto()
                {
                    IsSuccess = false,
                    Message = "Email is required.",
                };
                return BadRequest(res);
            }

              if (string.IsNullOrWhiteSpace(model.Role))
            {
                var res = new ResponseDto()
                {
                    IsSuccess = false,
                    Message = "Role is required.",
                };
                return BadRequest(res);
            }

            var assignRoleSuccessful = await _authService.AssignRole(model.Email, model.Role.ToUpper());
            if (!assignRoleSuccessful)
            {
                var res = new ResponseDto()
                {
                    IsSuccess = false,
                    Message = "Error assigning role.",
                };
                return BadRequest(res);
            }

            return Ok(new ResponseDto());
        }
    }
}