using CitiesApp.Domain.City;
using CitiesApp.IntegrationTests.Setup;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace CitiesApp.IntegrationTests.Cities
{
    public class RemoveCity : IntegrationTest
    {
        public RemoveCity(CustomWebApplicationFactory<Program> webApplicationFactory) : base(webApplicationFactory)
        {
        }

        [Fact]
        public async Task Ok()
        {
            //arrange
            var city = new City("Villach") { Location = new NetTopologySuite.Geometries.Point(46.61028, 13.85583) };
            await Db.Cities.AddAsync(city);
            await Db.SaveChangesAsync();

            //act
            var response = await HttpClient.DeleteAsync($"/api/cities/{city.Id}");

            //assert
            response.EnsureSuccessStatusCode();

            var removedCity = await Db.Cities.FirstOrDefaultAsync(x => x.Id == city.Id);
            removedCity.Should().BeNull();
        }


        [Fact]
        public async Task NotFound()
        {
            //act
            var response = await HttpClient.DeleteAsync($"/api/cities/{Guid.NewGuid()}");

            //assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }
    }
}
