using Microsoft.EntityFrameworkCore;
using CitiesApp.Application.Queries;
using CitiesApp.Infrastructure.Database;

namespace CitiesApp.Application.Cities.GetCitiesWithinDistance
{
    public class GetCitiesWithinDistanceQueryHandler : IQueryHandler<GetCitiesWithinDistanceQuery, string[]>
    {
        public ApplicationDbContext _db { get; set; }

        public GetCitiesWithinDistanceQueryHandler(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<string[]> Handle(GetCitiesWithinDistanceQuery request, CancellationToken cancellationToken)
        {
            var city = await _db.Cities.FirstOrDefaultAsync(c => c.Name == request.Name);
            if (city == null)
                return Array.Empty<string>();

            return await _db.Cities.Where(c => c.Location.IsWithinDistance(city.Location, 1000 * request.Distance) && c.Name != city.Name)
                                   .Select(c => c.Name)
                                   .ToArrayAsync();
        }
    }
}
