using CitiesApp.Application.Cities.AddCity;
using CitiesApp.Application.Commands;
using MediatR;
using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace CitiesApp.Application.Cities.AddSearchedCity
{
    public class AddSearchedCityCommandHandler : ICommandHandler<AddSearchedCityCommand>
    {
        private const string GeolocationApiUrl = "https://geocode.maps.co/search";
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IMediator _mediator;

        public AddSearchedCityCommandHandler(IHttpClientFactory httpClientFactory, IMediator mediator)
        {
            _httpClientFactory = httpClientFactory;
            _mediator = mediator;
        }

        public async Task Handle(AddSearchedCityCommand request, CancellationToken cancellationToken)
        {
            HttpClient client = _httpClientFactory.CreateClient();

            var uri = new Uri(GeolocationApiUrl + $"?q={request.Query}");

            var res = await client.GetAsync(uri);

            res.EnsureSuccessStatusCode();

            var locations = await res.Content.ReadFromJsonAsync<GeolocationApiResponse[]>();

            GeolocationApiResponse location = locations?.FirstOrDefault();

            await _mediator.Send(new AddCityCommand(location.Name, location.Latitude, location.Longitude));
        }

        private class GeolocationApiResponse
        {
            [JsonPropertyName("display_name")]
            public string Name { get; set; }
            [JsonPropertyName("lat")]
            public double Latitude { get; set; }
            [JsonPropertyName("lon")]
            public double Longitude { get; set; }
        }
    }
}
