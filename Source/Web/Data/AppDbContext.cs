using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Wingrid.Models;

namespace Wingrid.Data
{
    public class AppDbContext(DbContextOptions options) : IdentityDbContext<ApplicationUser>(options)
    {
        public DbSet<Team> Teams { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Entry> Entries { get; set; }
        public DbSet<Fixture> Fixtures { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<UserStatistics> UserStatistics { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Entry>().OwnsMany(p => p.EventEntries);

            modelBuilder.Entity<Team>(entity =>
            {
                entity.HasIndex(e => new { e.League, e.EspnId }).IsUnique();
            });

            modelBuilder.Entity<ApplicationUser>().HasOne(e => e.Statistics).WithOne(e => e.User);
        }
    }
}