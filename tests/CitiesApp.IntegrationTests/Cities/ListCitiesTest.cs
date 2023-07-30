using CitiesApp.Domain.City;
using CitiesApp.IntegrationTests.Setup;
using FluentAssertions;
using System.Text.Json;
using Xunit.Abstractions;

namespace CitiesApp.IntegrationTests.Cities
{
    public class ListCitiesTest : IntegrationTest
    {
        private readonly ITestOutputHelper _outputHelper;
        public ListCitiesTest(CustomWebApplicationFactory<Program> webApplicationFactory, ITestOutputHelper outputHelper) : base(webApplicationFactory)
        {
            _outputHelper = outputHelper;
        }

        [Fact]
        public async Task ListCities_EmptyList()
        {
            _outputHelper.WriteLine("Test query cities. Empty db");
            //arrange

            //act
            var response = await HttpClient.GetAsync("/api/cities");

            //assert
            response.EnsureSuccessStatusCode();
            var responseObj = await response.Content.ReadAsStringAsync();
            var jsonResponse = JsonSerializer.Deserialize<string[]>(responseObj);
            jsonResponse.Should().BeEmpty();
        }

        [Fact]
        public async Task ListCities_NotEmpty()
        {
            _outputHelper.WriteLine("Test query cities. Db filled");
            //arrange
            var cities = new List<City>
            {
                new City("Wien") { Location = new NetTopologySuite.Geometries.Point(12.5, 42.2) },
                new City("Warsaw") { Location = new NetTopologySuite.Geometries.Point(12.5, 42.2) },
                new City("Salzburg") { Location = new NetTopologySuite.Geometries.Point(12.5, 42.2) }
            };
            await Db.AddRangeAsync(cities);
            await Db.SaveChangesAsync();

            //act
            var response = await HttpClient.GetAsync("/api/cities");

            //assert
            response.EnsureSuccessStatusCode();
            var responseObj = await response.Content.ReadAsStringAsync();
            var jsonResponse = JsonSerializer.Deserialize<string[]>(responseObj);
            jsonResponse.Should().BeEquivalentTo(cities.Select(c => c.Name).ToArray());
        }
    }
}
