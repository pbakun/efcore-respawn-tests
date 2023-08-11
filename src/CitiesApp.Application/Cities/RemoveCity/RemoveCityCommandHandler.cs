using CitiesApp.Application.Commands;
using CitiesApp.Domain.City;
using CitiesApp.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace CitiesApp.Application.Cities.RemoveCity
{
    public class RemoveCityCommandHandler : ICommandHandler<RemoveCityCommand>
    {
        private readonly ApplicationDbContext _dbContext;

        public RemoveCityCommandHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Handle(RemoveCityCommand request, CancellationToken cancellationToken)
        {
            var city = await _dbContext.Cities.FirstOrDefaultAsync(x => x.Id == request.CityId);
            if (city == null)
                throw new Exception("City does not exist");
            _dbContext.Remove<City>(city);
            await _dbContext.SaveChangesAsync();
        }
    }
}
