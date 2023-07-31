using CitiesApp.Infrastructure.Database;
using Microsoft.Extensions.DependencyInjection;

namespace CitiesApp.IntegrationTests.Setup
{
    [Collection(nameof(IntegrationTestCollection))]
    public class IntegrationTest : IAsyncLifetime
    {
        public readonly CustomWebApplicationFactory<Program> WebApplicationFactory;
        public readonly IServiceScope Scope;
        public readonly ApplicationDbContext Db;
        public readonly HttpClient HttpClient;

        public IntegrationTest(CustomWebApplicationFactory<Program> webApplicationFactory)
        {
            WebApplicationFactory = webApplicationFactory;
            Scope = webApplicationFactory.Services.CreateScope();
            Db = Scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            HttpClient = webApplicationFactory.CreateClient();
        }

        public virtual Task InitializeAsync() => Task.CompletedTask;

        public virtual async Task DisposeAsync()
        {
            await WebApplicationFactory.ResetDatabaseAsync();
            Scope.Dispose();
        }
    }
}
