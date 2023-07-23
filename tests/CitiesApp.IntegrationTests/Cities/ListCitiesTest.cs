using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;
using CitiesApp.Infrastructure.Database;
using CitiesApp.IntegrationTests.Setup;
using Microsoft.EntityFrameworkCore;
using Xunit.Abstractions;

namespace CitiesApp.IntegrationTests.Cities
{
    [Collection(nameof(IntegrationTestCollection))]
    public class ListCitiesTest : IAsyncLifetime
    {
        private readonly CustomWebApplicationFactory<Program> _webApplicationFactory;
        private readonly ITestOutputHelper _outputHelper;
        private readonly IServiceScope _scope;
        private readonly ApplicationDbContext _db;
        private readonly HttpClient _httpClient;
        public ListCitiesTest(CustomWebApplicationFactory<Program> webApplicationFactory, ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
            _webApplicationFactory = webApplicationFactory;
            _scope = webApplicationFactory.Services.CreateScope();
            _db = _scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            _httpClient = webApplicationFactory.CreateClient();
        }

        [Fact]
        public async Task AddCity()
        {
            _outputHelper.WriteLine("Test Add Villach to db");
            //arrange
            var response = await _httpClient.PostAsync($"/api/cities?name=Villach", null);

            //assert
            response.EnsureSuccessStatusCode();
            var city = await _db.Cities.ToListAsync();
            city.Should().HaveCount(1);
            city.FirstOrDefault()?.Name.Should().Be("Villach");
        }

        [Fact]
        public async Task ListCities_DefinedCities()
        {
            _outputHelper.WriteLine("Test query cities");
            //arrange
            //_db.Cities.Add(new Domain.City.City("VIllach"));
            //_db.SaveChanges();


            //act
            //var cities = _db.Cities.ToList();
            var response = await _httpClient.GetAsync("/api/cities");

            //assert
            response.EnsureSuccessStatusCode();
            var responseObj = await response.Content.ReadAsStringAsync();
            var jsonResponse = JsonSerializer.Deserialize<string[]>(responseObj);
            jsonResponse.Should().BeEmpty();
        }

        [Fact]
        public async Task ZAddCity()
        {
            _outputHelper.WriteLine("Test query cities");
            //arrange
            //_db.Cities.Add(new Domain.City.City("VIllach"));
            //_db.SaveChanges();


            //act
            //var cities = _db.Cities.ToList();
            var response = await _httpClient.GetAsync("/api/cities");

            //assert
            response.EnsureSuccessStatusCode();
            var responseObj = await response.Content.ReadAsStringAsync();
            var jsonResponse = JsonSerializer.Deserialize<string[]>(responseObj);
            jsonResponse.Should().BeEmpty();
        }

        [Fact]
        public async Task ZZListCities_DefinedCitieZZs()
        {
            _outputHelper.WriteLine("Test Add Klagenfurt to db");
            //arrange
            var response = await _httpClient.PostAsync($"/api/cities?name=Klagenfurt", null);

            //assert
            response.EnsureSuccessStatusCode();
            var city = await _db.Cities.ToListAsync();
            city.Should().HaveCount(1);
            city.FirstOrDefault()?.Name.Should().Be("Klagenfurt");
        }

        public async Task InitializeAsync() => await _webApplicationFactory.ResetDatabaseAsync();

        public Task DisposeAsync()
        {
            _scope.Dispose();
            return Task.CompletedTask;
        }
    }
}
