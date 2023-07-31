using CitiesApp.Domain.City;
using CitiesApp.IntegrationTests.Setup;
using FluentAssertions;
using System.Text.Json;

namespace CitiesApp.IntegrationTests.Cities
{
    public class GetCitiesWithinDistance : IntegrationTest
    {
        public GetCitiesWithinDistance(CustomWebApplicationFactory<Program> webApplicationFactory) : base(webApplicationFactory)
        {
        }

        [Fact]
        public async Task Ok()
        {
            //arrange
            var referenceCity = new City("Klagenfurt") { Location = new NetTopologySuite.Geometries.Point(46.62472, 14.30528) };
            var citiesInRange = new List<City>
            {
                new City("Villach") { Location = new NetTopologySuite.Geometries.Point(46.61028, 13.85583) },
                new City("Spittal an der Drau") { Location = new NetTopologySuite.Geometries.Point(46.8, 13.5) },
                new City("Bled") { Location = new NetTopologySuite.Geometries.Point(46.36917, 14.11361) },
            };

            var citiesOutOfRange = new List<City>
            {
                new City("Beijing") { Location = new NetTopologySuite.Geometries.Point(39.9075, 116.39723) },
                new City("Paris") { Location = new NetTopologySuite.Geometries.Point(48.85341, 2.3488) },
                new City("Rome") { Location = new NetTopologySuite.Geometries.Point(41.89193, 12.51133) },
            };
            await Db.Cities.AddAsync(referenceCity);
            await Db.Cities.AddRangeAsync(citiesInRange);
            await Db.Cities.AddRangeAsync(citiesOutOfRange);
            await Db.SaveChangesAsync();

            //act
            var response = await HttpClient.GetAsync($"/api/cities/Klagenfurt?distance=150");

            //assert
            response.EnsureSuccessStatusCode();
            var responseObj = await response.Content.ReadAsStringAsync();
            var jsonResponse = JsonSerializer.Deserialize<string[]>(responseObj);
            jsonResponse.Should().BeEquivalentTo(citiesInRange.Select(c => c.Name).ToArray());
        }
    }
}
