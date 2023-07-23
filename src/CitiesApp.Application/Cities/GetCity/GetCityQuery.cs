using CitiesApp.Application.Queries;
using CitiesApp.Domain.City;

namespace CitiesApp.Application.Cities.GetCity
{
    public class GetCityQuery : IQuery<string[]>
    {
        public string Name { get; set; }

        public GetCityQuery(string name)
        {
            Name = name;
        }
    }
}
