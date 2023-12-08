using Microsoft.EntityFrameworkCore;
using Wingrid.Services.EventAPI.Models;

namespace Wingrid.Services.EventAPI.Data
{
    public class AppDbContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<Team> Teams { get; set; }
        public DbSet<Event> Events { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Team>(entity =>
            {
                entity.HasIndex(e => new { e.League, e.EspnId }).IsUnique();
            });
        }
    }
}