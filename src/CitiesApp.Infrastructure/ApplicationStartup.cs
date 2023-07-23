using CitiesApp.Infrastructure.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace CitiesApp.Infrastructure
{
    public static class ApplicationStartup
    {
        public static void AddInfrastructure(this IServiceCollection services, string databaseConnectionString)
        {
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblyContaining(typeof(IRequestHandler<,>));
                cfg.RegisterServicesFromAssembly(Assembly.Load("CitiesApp.Application"));
            });

            services.AddDbContext<ApplicationDbContext>(opt =>
                opt.UseNpgsql(databaseConnectionString, o => o.UseNetTopologySuite()),
                ServiceLifetime.Scoped);

            services.AddHttpClient();

        }

        public static void Initialize(IServiceScope scope)
        {
            IServiceProvider provider = scope.ServiceProvider;

            var dbContext = provider.GetRequiredService<ApplicationDbContext>();
            dbContext.Database.Migrate();
        }
    }
}
