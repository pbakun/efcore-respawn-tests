using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitiesApp.Application.Queries;
using CitiesApp.Domain.City;

namespace CitiesApp.Application.Cities.ListCities
{
    public class ListCitiesQuery : IQuery<string[]>
    {
        public string Name { get; set; }
        public ListCitiesQuery(string name)
        {
            Name = name;
        }
    }
}
