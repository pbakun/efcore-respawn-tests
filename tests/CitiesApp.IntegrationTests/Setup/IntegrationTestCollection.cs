using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CitiesApp.IntegrationTests.Setup
{
    [CollectionDefinition(nameof(IntegrationTestCollection), DisableParallelization = true)]
    public class IntegrationTestCollection : ICollectionFixture<CustomWebApplicationFactory<Program>>
    {
    }
}
