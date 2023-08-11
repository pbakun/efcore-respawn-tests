using Microsoft.EntityFrameworkCore;

namespace CitiesApp.Infrastructure.Database
{
    public static class ApplicationDbContextExtensions
    {
        public static void ConfigureOptions(this DbContextOptionsBuilder builder, string connectionString)
        {
            builder.UseNpgsql(connectionString, o => o.UseNetTopologySuite());
        }
    }
}
