namespace CitiesApp.IntegrationTests.Setup
{
    [CollectionDefinition(nameof(IntegrationTestCollection), DisableParallelization = true)]
    public class IntegrationTestCollection : ICollectionFixture<CustomWebApplicationFactory<Program>>
    {
    }
}
