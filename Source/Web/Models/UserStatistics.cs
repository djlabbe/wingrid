
using Microsoft.EntityFrameworkCore;

namespace Wingrid.Models
{
    [PrimaryKey(nameof(UserId))]
    public class UserStatistics
    {
        public ApplicationUser? User { get; set; }
        public required string UserId { get; set; }
        public int TotalCollegePicks { get; set; }
        public int TotalProPicks { get; set; }
        public int CorrectCollegePicks { get; set; }
        public int CorrectProPicks { get; set; }

        public decimal CollegePercentage => CorrectCollegePicks / TotalCollegePicks;
        public decimal ProPercentage => CorrectProPicks / TotalProPicks;
        public decimal TotalPercentage => (CorrectCollegePicks + CorrectProPicks) / (TotalCollegePicks + TotalProPicks);
    }
}