using Microsoft.EntityFrameworkCore;
using Wingrid.Services.EventAPI.Models;

namespace Wingrid.Services.EventAPI.Data
{
    public class AppDbContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<Team> Teams { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Entry> Entries { get; set; }
        public DbSet<Fixture> Fixtures { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Entry>().OwnsMany(p => p.EventEntries);

            modelBuilder.Entity<Team>(entity =>
            {
                entity.HasIndex(e => new { e.League, e.EspnId }).IsUnique();
            });
        }
    }
}