using CitiesApp.Application.Queries;

namespace CitiesApp.Application.Cities.GetCitiesWithinDistance
{
    public class GetCitiesWithinDistanceQuery : IQuery<string[]>
    {
        public string Name { get; set; }
        /// <summary>
        /// Distance in kilometers
        /// </summary>
        public double Distance { get; set; }

        public GetCitiesWithinDistanceQuery(string name, double distance)
        {
            Name = name;
            Distance = distance;
        }
    }
}
