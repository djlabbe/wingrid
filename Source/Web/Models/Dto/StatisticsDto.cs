using Wingrid.Models.Dto;

namespace Web.Models.Dto
{
    public class StatisticsDto()
    {
        public UserDto? User { get; set; }
        public int Entries { get; set; }
        public int Wins { get; set; }

        public int TotalCollegePicks { get; set; }
        public int TotalProPicks { get; set; }
        public int CorrectCollegePicks { get; set; }
        public int CorrectProPicks { get; set; }
        public int TotalTieBreakerError { get; set; }

        public decimal? CollegePercentage { get; set; }
        public decimal? ProPercentage { get; set; }
        public decimal? TotalPercentage { get; set; }
        public decimal? WinPercentage { get; set; }
        public decimal? AverageTieBreakerError { get; set; }
    }
}