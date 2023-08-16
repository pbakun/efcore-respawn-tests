using CitiesApp.Application.Commands;
using CitiesApp.Domain.Exception;
using CitiesApp.Infrastructure.Database;

namespace CitiesApp.Application.Cities.UpdateCity
{
    public class UpdateCityCommandHandler : ICommandHandler<UpdateCityCommand>
    {
        private readonly ApplicationDbContext _db;

        public UpdateCityCommandHandler(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task Handle(UpdateCityCommand request, CancellationToken cancellationToken)
        {
            var city = await _db.Cities.FindAsync(request.CityId, cancellationToken);
            if (city == null)
            {
                throw new EntityNotFoundException();
            }

            city.Name = request.Name;
            await _db.SaveChangesAsync();
        }
    }
}
