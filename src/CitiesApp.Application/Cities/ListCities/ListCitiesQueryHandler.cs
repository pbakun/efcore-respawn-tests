using Microsoft.EntityFrameworkCore;
using CitiesApp.Application.Queries;
using CitiesApp.Domain.City;
using CitiesApp.Infrastructure.Database;

namespace CitiesApp.Application.Cities.ListCities
{
    public class ListCitiesQueryHandler : IQueryHandler<ListCitiesQuery, string[]>
    {
        public ApplicationDbContext _db { get; set; }

        public ListCitiesQueryHandler(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<string[]> Handle(ListCitiesQuery request, CancellationToken cancellationToken)
        {
            return await _db.Cities.Select(c => c.Name).ToArrayAsync();
        }
    }
}
