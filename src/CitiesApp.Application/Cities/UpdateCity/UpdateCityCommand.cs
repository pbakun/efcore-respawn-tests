using CitiesApp.Application.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CitiesApp.Application.Cities.UpdateCity
{
    public class UpdateCityCommand : CommandBase
    {
        public Guid CityId { get; set; }
        public string Name { get; set; }
        public UpdateCityCommand(Guid cityId, string name)
        {
            CityId = cityId;
            Name = name;
        }
    }
}
