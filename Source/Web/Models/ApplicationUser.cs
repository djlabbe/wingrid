using Microsoft.AspNetCore.Identity;

namespace Wingrid.Models;

public class ApplicationUser : IdentityUser
{
    public string? Name { get; set; }
    public UserStatistics? Statistics { get; set; }
}
