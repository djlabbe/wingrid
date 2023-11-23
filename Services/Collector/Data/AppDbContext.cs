using Microsoft.EntityFrameworkCore;
using Wingrid.Services.Collector.Models;

namespace Wingrid.Services.Collector.Data
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