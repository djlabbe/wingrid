
using Microsoft.EntityFrameworkCore;

namespace Wingrid.Models
{
    [PrimaryKey(nameof(UserId))]
    public class UserStatistics(string userId)
    {
        public ApplicationUser? User { get; set; }
        public string UserId { get; set; } = userId;

        public int Entries { get; set; }
        public int Wins { get; set; }

        public int TotalCollegePicks { get; set; }
        public int TotalProPicks { get; set; }
        public int CorrectCollegePicks { get; set; }
        public int CorrectProPicks { get; set; }
        public int TotalTieBreakerError { get; set; }

        public decimal CollegePercentage => CorrectCollegePicks / TotalCollegePicks;
        public decimal ProPercentage => CorrectProPicks / TotalProPicks;
        public decimal TotalPercentage => (CorrectCollegePicks + CorrectProPicks) / (TotalCollegePicks + TotalProPicks);
        public decimal WinPercentage => Wins / Entries;
        public decimal AverageTieBreakerError => TotalTieBreakerError / Entries;
    }
}