namespace CitiesApp.Infrastructure.Exceptions
{
    public class MissingApplicationConfigurationException : Exception
    {
        public MissingApplicationConfigurationException(string? name) : base($"Missing required configuration: {name}")
        {
        }
    }
}
