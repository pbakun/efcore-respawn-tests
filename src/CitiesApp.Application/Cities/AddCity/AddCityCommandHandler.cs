using NetTopologySuite.Geometries;
using CitiesApp.Application.Commands;
using CitiesApp.Domain.City;
using CitiesApp.Infrastructure.Database;

namespace CitiesApp.Application.Cities.AddCity
{
    public class AddCityCommandHandler : ICommandHandler<AddCityCommand>
    {
        private readonly ApplicationDbContext _db;

        public AddCityCommandHandler(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task Handle(AddCityCommand request, CancellationToken cancellationToken)
        {
            var city = new City(request.Name);
            city.Location = new Point(request.Latitude, request.Longitude);
            await _db.AddAsync<City>(city, cancellationToken);
            await _db.SaveChangesAsync();
        }
    }
}
