using Microsoft.EntityFrameworkCore;
using CitiesApp.Application.Queries;
using CitiesApp.Infrastructure.Database;

namespace CitiesApp.Application.Cities.GetCity
{
    public class GetCityQueryHandler : IQueryHandler<GetCityQuery, string[]>
    {
        public ApplicationDbContext _db { get; set; }

        public GetCityQueryHandler(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<string[]> Handle(GetCityQuery request, CancellationToken cancellationToken)
        {
            var city = await _db.Cities.FirstOrDefaultAsync(c => c.Name == request.Name);
            if (city == null)
                return Array.Empty<string>();

            return await _db.Cities.Where(c => c.Location.IsWithinDistance(city.Location, 1000 * 450))
                                   .Select(c => c.Name)
                                   .ToArrayAsync();
        }
    }
}
