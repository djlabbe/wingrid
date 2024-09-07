
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

        public decimal? CollegePercentage => TotalCollegePicks > 0 ? CorrectCollegePicks / TotalCollegePicks : null;
        public decimal? ProPercentage => TotalProPicks > 0 ? CorrectProPicks / TotalProPicks : null;
        public decimal? TotalPercentage => (TotalCollegePicks + TotalProPicks) > 0 ? ((CorrectCollegePicks + CorrectProPicks) / (TotalCollegePicks + TotalProPicks)) : null;
        public decimal? WinPercentage => Entries > 0 ? Wins / Entries : null;
        public decimal? AverageTieBreakerError => Entries > 0 ? TotalTieBreakerError / Entries : null;
    }
}