using Domain;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Event> Events { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }

        public T SafeAttach<T>(T entity, Func<T, bool> searchFunc) where T : class
        {
            return SearchLocal(searchFunc) ?? Attach(entity).Entity;
        }

         public T SafeAdd<T>(T entity, Func<T, bool> searchFunc) where T : class
        {
            return SearchLocal(searchFunc) ?? Add(entity).Entity;
        }

        private T? SearchLocal<T>(Func<T, bool> searchFunc) where T : class
        {
            return Set<T>().Local.SingleOrDefault(searchFunc);
        }
    }
}