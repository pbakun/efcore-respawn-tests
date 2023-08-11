using Microsoft.EntityFrameworkCore;
using CitiesApp.Domain.City;

namespace CitiesApp.Infrastructure.Database
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<City> Cities { get; set; }
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.HasPostgresExtension("postgis");

            builder.Entity<City>()
                .Property(b => b.Location)
                .HasColumnType("geography (point)");
            
        }
    }
}
