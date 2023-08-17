using MediatR;
using Microsoft.AspNetCore.Mvc;
using CitiesApp.Application.Cities.AddCity;
using CitiesApp.Application.Cities.GetCitiesWithinDistance;
using CitiesApp.Application.Cities.ListCities;
using CitiesApp.Application.Cities.AddSearchedCity;
using CitiesApp.Domain.City;
using CitiesApp.Application.Cities.RemoveCity;
using CitiesApp.Application.Cities.UpdateCity;
using CitiesApp.Presentation.Model.City;

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
        public async Task<IActionResult> GetCitiesWithinDistance(string name, [FromQuery]double distance)
        {
            var cities = await _mediator.Send(new GetCitiesWithinDistanceQuery(name, distance));
            return Ok(cities);
        }

        [HttpGet]
        public async Task<IActionResult> ListCities()
        {
            var cities = await _mediator.Send(new ListCitiesQuery());
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

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> RemoveCity(string id)
        {
            if(!Guid.TryParse(id, out Guid cityId))
            {
                return BadRequest();
            }

            await _mediator.Send(new RemoveCityCommand(cityId));
            return Ok();
        }

        [HttpPatch]
        [Route("{id}")]
        public async Task<IActionResult> UpdateCity(string id, [FromBody]UpdateCity city)
        {
            if (!Guid.TryParse(id, out Guid cityId))
            {
                return BadRequest();
            }

            await _mediator.Send(new UpdateCityCommand(cityId, city.Name));
            return Ok();
        }
    }
}
