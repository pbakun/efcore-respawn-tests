using MediatR;
using Microsoft.AspNetCore.Mvc;
using CitiesApp.Application.Cities.AddCity;
using CitiesApp.Application.Cities.GetCity;
using CitiesApp.Application.Cities.ListCities;
using CitiesApp.Application.Cities.AddSearchedCity;

namespace CitiesApp.Controllers
{
    [ApiController]
    [Route("api/cities")]
    public class CitiesController : Controller
    {
        private readonly IMediator _mediator;

        public CitiesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("{name}")]
        public async Task<IActionResult> GetCity(string name)
        {
            var city = await _mediator.Send(new GetCityQuery(name));
            return Ok(city);
        }

        [HttpGet]
        public async Task<IActionResult> ListCities()
        {
            var cities = await _mediator.Send(new ListCitiesQuery(""));
            return Ok(cities);
        }

        [HttpPost]
        public IActionResult AddCity(string name, double latitude, double longitude)
        {
            _mediator.Send(new AddCityCommand(name, latitude, longitude));

            return Ok();
        }

        [HttpPost]
        [Route("search")]
        public IActionResult AddSearchedCity(string query)
        {
            _mediator.Send(new AddSearchedCityCommand(query));

            return Ok();
        }
    }
}
