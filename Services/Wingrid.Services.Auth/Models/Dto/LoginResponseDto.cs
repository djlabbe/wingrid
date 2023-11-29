using Wingrid.Services.Auth.Models.Dto;

namespace Auth.Models.Dto
{
    public class LoginResponseDto
    {
        public UserDto? User { get; set; }
        public List<string>? Roles {get; set;}
        public string? Token { get; set; }
    }
}