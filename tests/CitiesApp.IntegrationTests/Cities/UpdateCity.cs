using CitiesApp.Domain.City;
using CitiesApp.IntegrationTests.Setup;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Json;

namespace CitiesApp.IntegrationTests.Cities
{
    public class UpdateCity : IntegrationTest
    {
        public UpdateCity(CustomWebApplicationFactory<Program> webApplicationFactory) : base(webApplicationFactory)
        {
        }

        [Fact]
        public async Task Ok()
        {
            //arrange
            var city = new City("Klagenfurt") { Location = new NetTopologySuite.Geometries.Point(46.62472, 14.30528) };
            await Db.Cities.AddAsync(city);
            await Db.SaveChangesAsync();

            //act
            var payload = new Presentation.Model.City.UpdateCity
            {
                Name = "Klagenfurt am Wörthersee"
            };

            var response = await HttpClient.PatchAsJsonAsync($"/api/cities/{city.Id}", payload);

            //assert
            response.EnsureSuccessStatusCode();

            var updated = await Db.Cities.FirstOrDefaultAsync(c => c.Id == city.Id);
            updated?.Name.Should().Be(payload.Name);
        }


        [Fact]
        public async Task NotFound()
        {
            //act
            var payload = new Presentation.Model.City.UpdateCity
            {
                Name = "Klagenfurt am Wörthersee"
            };

            var response = await HttpClient.PatchAsJsonAsync($"/api/cities/{Guid.NewGuid()}", payload);

            //assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }
    }
}
