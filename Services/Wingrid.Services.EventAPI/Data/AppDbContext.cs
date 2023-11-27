using Microsoft.EntityFrameworkCore;
using Wingrid.Services.EventAPI.Models;

namespace Wingrid.Services.EventAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {

        }
        
        public DbSet<Team> Teams { get; set; }
        public DbSet<Event> Events { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}