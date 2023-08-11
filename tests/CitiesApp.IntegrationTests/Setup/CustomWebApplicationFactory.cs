using CitiesApp.Infrastructure;
using CitiesApp.Infrastructure.Database;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Respawn;
using Respawn.Graph;
using System.Data.Common;

namespace CitiesApp.IntegrationTests.Setup
{
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup>, IAsyncLifetime where TStartup : class
    {
        public string DbConnectionString { get; private set; }

        private const int PostgresPort = 5432;
        private readonly IContainer _dbContainer;

        private Respawner respawner = default!;
        private DatabaseFacade _databaseFacade = default!;

        public CustomWebApplicationFactory()
        {
            _dbContainer = new ContainerBuilder().WithImage("postgis/postgis:latest")
                                                .WithPortBinding(PostgresPort, true)
                                                .WithEnvironment("POSTGRES_USER", "root")
                                                .WithEnvironment("POSTGRES_PASSWORD", "mypassword")
                                                .WithEnvironment("POSTGRES_DB", "integration_tests")
                                                .WithEnvironment("PGPORT", PostgresPort.ToString())
                                                .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(PostgresPort))
                                                .WithCleanUp(true)
                                                .Build();
        }
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
                if (descriptor != null)
                    services.Remove(descriptor);
                services.AddInfrastructure(DbConnectionString);

                var provider = services.BuildServiceProvider();
                using var scope = provider.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                _databaseFacade = context.Database;

                GetOpenedDbConnectionAsync().Wait();

                context.Database.Migrate();
                InitRespawner().Wait();
            });
        }


        public async Task InitRespawner()
        {
            DbConnection conn = await GetOpenedDbConnectionAsync();
            respawner = await Respawner.CreateAsync(conn, new RespawnerOptions
            {
                SchemasToInclude = new[] { "public" },
                TablesToIgnore = new Table[]
                {
                    "__EFMigrationsHistory",
                    "spatial_ref_sys"
                },
                DbAdapter = DbAdapter.Postgres
            });
        }

        public async Task ResetDatabaseAsync()
        {
            DbConnection conn = await GetOpenedDbConnectionAsync();
            await respawner.ResetAsync(conn);
        }

        public async Task InitializeAsync()
        {
            await _dbContainer.StartAsync().ConfigureAwait(false);
            DbConnectionString = $"Host={_dbContainer.Hostname};Port={_dbContainer.GetMappedPublicPort(PostgresPort)};Database=integration_tests; Username=root; Password=mypassword;Pooling=true;Maximum Pool Size=1024;";
        }

        async Task IAsyncLifetime.DisposeAsync()
        {
            await _dbContainer.StopAsync();
            await _dbContainer.DisposeAsync();
        }

        private async Task<DbConnection> GetOpenedDbConnectionAsync()
        {
            var conn = _databaseFacade.GetDbConnection();
            if (conn.State != System.Data.ConnectionState.Open)
                await conn.OpenAsync();
            return conn;
        }
    }
}
