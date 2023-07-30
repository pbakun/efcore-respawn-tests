using CitiesApp.Infrastructure.Database;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

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

        public Task InitializeAsync() => Task.CompletedTask;

        public async Task DisposeAsync()
        {
            await WebApplicationFactory.ResetDatabaseAsync();
            Scope.Dispose();
        }
    }
}
