using CitiesApp.Application.Commands;

namespace CitiesApp.Application.Cities.AddSearchedCity
{
    public class AddSearchedCityCommand : CommandBase
    {
        public string Query { get; set; }
        public AddSearchedCityCommand(string query)
        {
            this.Query = query;
        }
    }
}
