
using Microsoft.EntityFrameworkCore;
using Wingrid.Services.Collector.Data;
using Wingrid.Services.Collector.Models;

namespace Wingrid.Services.Collector.Services
{
    public interface ITeamsService
    {
        public Task<IEnumerable<Team>> GetTeamsAsync();
        public Task<Team?> GetTeamAsync(string id);
        public Team AddTeam(Team team);
        public Task<int> SaveChangesAsync();
    }

    public class TeamsService : ITeamsService
    {
        private readonly AppDbContext _context;

        public TeamsService(AppDbContext context)
        {
            _context = context;
        }


        public async Task<IEnumerable<Team>> GetTeamsAsync()
        {
            var teams = await _context.Teams.ToListAsync();
            return teams;
        }

        public Team AddTeam(Team team)
        {
            var saved = _context.Teams.Add(team).Entity;
            return saved;
        }

        public Task<int> SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }

        public async Task<Team?> GetTeamAsync(string id)
        {
           var team = await _context.Teams.FindAsync(id);
           return team;
        }
    }
}