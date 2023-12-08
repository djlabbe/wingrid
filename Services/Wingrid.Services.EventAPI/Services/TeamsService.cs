
using Microsoft.EntityFrameworkCore;
using Wingrid.Services.EventAPI.Data;
using Wingrid.Services.EventAPI.Models;

namespace Wingrid.Services.EventAPI.Services
{
    public interface ITeamsService
    {
        public Task<IEnumerable<Team>> GetTeamsAsync();
        public Task<IEnumerable<Team>> GetTeamsByLeagueAsync(League league);
        public Task<Team?> GetTeamAsync(string id);
        public Team AddTeam(Team team);
        public Task<int> SaveChangesAsync();
    }

    public class TeamsService(AppDbContext context) : ITeamsService
    {
        private readonly AppDbContext _context = context;

        public async Task<IEnumerable<Team>> GetTeamsAsync()
        {
            var teams = await _context.Teams.ToListAsync();
            return teams;
        }

        public async Task<IEnumerable<Team>> GetTeamsByLeagueAsync(League league)
        {
            var teams = await _context.Teams.Where(t => t.League == league).ToListAsync();
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