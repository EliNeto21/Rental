using DotNet.Testcontainers.Builders;
using Testcontainers.PostgreSql;

namespace Tests.IntegrationTests.Helpers
{
    public class PostgresFixture : IAsyncLifetime
    {
        private readonly PostgreSqlContainer _container;
        public string ConnectionString { get; private set; } = string.Empty;

        public PostgresFixture()
        {
            _container = new PostgreSqlBuilder()
                .WithDatabase("rental_test")
                .WithUsername("postgres")
                .WithPassword("postgres")
                .Build();
        }

        public async Task DisposeAsync()
        {
            await _container.DisposeAsync();
        }

        public async Task InitializeAsync()
        {
            await _container.StartAsync();
            ConnectionString = _container.GetConnectionString();
        }
    }
}
