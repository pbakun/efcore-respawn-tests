using CitiesApp.Application.Commands;

namespace CitiesApp.Application.Cities.AddCity
{
    public class AddCityCommand : CommandBase
    {
        public string Name { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public AddCityCommand(string name, double latitude, double longitude)
        {
            Name = name;
            Latitude = latitude;
            Longitude = longitude;
        }
    }
}
