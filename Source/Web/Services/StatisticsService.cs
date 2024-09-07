using System.Linq;
using Microsoft.EntityFrameworkCore;
using Wingrid.Data;
using Wingrid.Models;

namespace Web.Services
{
    public interface IStatisticsService
    {
        public Task<IEnumerable<UserStatistics>> GetStatistics();
    }

    public class StatisticsService(AppDbContext context) : IStatisticsService
    {
        private readonly AppDbContext _context = context;

        public async Task<IEnumerable<UserStatistics>> GetStatistics()
        {
            var statistics = await _context.UserStatistics.Include(us => us.User).ToListAsync();
            return statistics;
        }
    }
}