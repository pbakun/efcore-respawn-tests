using CitiesApp.IntegrationTests.Setup;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit.Abstractions;

namespace CitiesApp.IntegrationTests.Cities
{
    public class AddCity : IntegrationTest
    {
        private readonly ITestOutputHelper _outputHelper;
        public AddCity(CustomWebApplicationFactory<Program> webApplicationFactory, ITestOutputHelper outputHelper) : base(webApplicationFactory)
        {
            _outputHelper = outputHelper;
        }

        [Fact]
        public async Task AddSingleCity()
        {
            _outputHelper.WriteLine("Test Add Villach to db");
            //arrange
            var response = await HttpClient.PostAsync($"/api/cities?name=Villach", null);

            //assert
            response.EnsureSuccessStatusCode();
            var city = await Db.Cities.ToListAsync();
            city.Should().HaveCount(1);
            city.FirstOrDefault()?.Name.Should().Be("Villach");
        }
    }
}
