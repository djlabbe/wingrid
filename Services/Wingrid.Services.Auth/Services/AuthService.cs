using System.Security.Claims;
using Auth.Models;
using Auth.Models.Dto;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Wingrid.Auth.Data;
using Wingrid.Services.Auth.Models.Dto;

namespace Wingrid.Services.Auth.Services
{
    public interface IAuthService
    {
        Task<string> Register(RegistrationRequestDto registrationRequestDto);
        Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto);
        Task<LoginResponseDto> GetCurrentUser(string? email);
        Task<bool> AssignRole(string email, string roleName);
    }

    public class AuthService(AppDbContext db, IJwtTokenGenerator jwtTokenGenerator, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager) : IAuthService
    {
        private readonly AppDbContext _db = db;
        private readonly IJwtTokenGenerator _jwtTokenGenerator = jwtTokenGenerator;
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly RoleManager<IdentityRole> _roleManager = roleManager;

        public async Task<LoginResponseDto> GetCurrentUser(string? email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return new LoginResponseDto() { User = null, Token = "" };
            }

            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Email == email);
            if (user == null) return new LoginResponseDto() { User = null, Token = "" };
            UserDto userDto = new()
            {
                Email = user.Email,
                Id = user.Id,
                Name = user.Name,
                PhoneNumber = user.PhoneNumber,
            };

            var roles = await _userManager.GetRolesAsync(user);
            LoginResponseDto loginResponseDto = new()
            {
                User = userDto,
                Roles = [.. roles],
                Token = _jwtTokenGenerator.GenerateToken(user, roles),
            };

            return loginResponseDto;
        }

        public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
        {
            if (string.IsNullOrWhiteSpace(loginRequestDto.Username) || string.IsNullOrWhiteSpace(loginRequestDto.Password))
            {
                return new LoginResponseDto() { User = null, Token = "" };
            }

            var user = _db.ApplicationUsers.FirstOrDefault(u => u.UserName != null && u.UserName.ToUpper() == loginRequestDto.Username.ToUpper());
            if (user == null) return new LoginResponseDto() { User = null, Token = "" };

            bool isValid = await _userManager.CheckPasswordAsync(user, loginRequestDto.Password);
            if (!isValid) return new LoginResponseDto() { User = null, Token = "" };

            UserDto userDto = new()
            {
                Email = user.Email,
                Id = user.Id,
                Name = user.Name,
                PhoneNumber = user.PhoneNumber,
            };

            var roles = await _userManager.GetRolesAsync(user);
            LoginResponseDto loginResponseDto = new()
            {
                User = userDto,
                Roles = [.. roles],
                Token = _jwtTokenGenerator.GenerateToken(user, roles),
            };

            return loginResponseDto;
        }

        public async Task<string> Register(RegistrationRequestDto registrationRequestDto)
        {
            ApplicationUser user = new()
            {
                UserName = registrationRequestDto.Email,
                Email = registrationRequestDto.Email,
                NormalizedEmail = registrationRequestDto.Email?.ToUpper(),
                Name = registrationRequestDto.Name,
                PhoneNumber = registrationRequestDto.PhoneNumber,
            };

            try
            {
                var result = await _userManager.CreateAsync(user, registrationRequestDto.Password ?? "");
                if (result.Succeeded)
                {
                    var userToReturn = _db.ApplicationUsers.First(u => u.UserName == registrationRequestDto.Email);
                    UserDto userDto = new()
                    {
                        Email = userToReturn.Email,
                        Id = userToReturn.Id,
                        Name = userToReturn.Name,
                        PhoneNumber = userToReturn.PhoneNumber,
                    };
                    return "";
                }
                else
                {
                    return result.Errors.First().Description;
                }
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public async Task<bool> AssignRole(string email, string roleName)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(u => u.Email != null && u.Email.ToUpper() == email.ToUpper());
            if (user != null)
            {
                if (!_roleManager.RoleExistsAsync(roleName).GetAwaiter().GetResult())
                {
                    // Create new role
                    await _roleManager.CreateAsync(new IdentityRole(roleName));
                }
                await _userManager.AddToRoleAsync(user, roleName);
                return true;
            }
            return false;
        }
    }
}