using CitiesApp.Application.Cities.AddCity;
using CitiesApp.IntegrationTests.Setup;
using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace CitiesApp.IntegrationTests.Transaction
{
    public class TransactionTest : IntegrationTest
    {
        private readonly IDbContextTransaction _transaction;
        public TransactionTest(CustomWebApplicationFactory<Program> webApplicationFactory) : base(webApplicationFactory)
        {
            _transaction = Db.Database.BeginTransaction();
        }

        [Fact]
        public async Task Transaction()
        {
            var mediator = Scope.ServiceProvider.GetRequiredService<IMediator>();

            await mediator.Send(new AddCityCommand("Olsztyn", 53.2, 14.2));

            using (var conn = new NpgsqlConnection(Db.Database.GetConnectionString()))
            {
                await conn.OpenAsync();

                using var command = conn.CreateCommand();
                command.CommandText = "Select * from public.\"Cities\";";
                var reader = await command.ExecuteReaderAsync();

                reader.HasRows.Should().BeFalse();
            }

            var cities = await Db.Cities.ToListAsync();

            cities.Should().HaveCount(1);
        }

        public override async Task DisposeAsync()
        {
            await _transaction.CommitAsync();
            await base.DisposeAsync();
        }
    }
}
