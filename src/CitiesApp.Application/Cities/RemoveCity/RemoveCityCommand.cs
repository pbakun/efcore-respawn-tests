using CitiesApp.Application.Commands;

namespace CitiesApp.Application.Cities.RemoveCity
{
    public class RemoveCityCommand : CommandBase
    {
        public Guid CityId { get; set; }
        public RemoveCityCommand(Guid cityId)
        {
            CityId = cityId;
        }
    }
}
