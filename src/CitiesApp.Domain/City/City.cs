
using NetTopologySuite.Geometries;

namespace CitiesApp.Domain.City
{
    public class City
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Point Location { get; set; }

        public City(string name)
        {
            Id = Guid.NewGuid();
            Name = name;
        }

    }
}
