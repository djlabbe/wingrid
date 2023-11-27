using Microsoft.EntityFrameworkCore;
using Wingrid.Services.FixtureAPI.Models;

namespace Wingrid.Services.FixtureAPI.Data
{
    public class AppDbContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<Fixture> Fixtures { get; set; }
        public DbSet<Entry> Entries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Entry>().OwnsMany(p => p.EventEntries);
        }
    }
}