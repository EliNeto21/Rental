using Infra.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Tests.IntegrationTests.Helpers
{
    public abstract class IntegrationTestBase : IClassFixture<PostgresFixture>, IDisposable
    {
        protected readonly HttpClient Client;
        private readonly ApiFactory _factory;

        public IntegrationTestBase(PostgresFixture fixture)
        {
            _factory = new ApiFactory(fixture.ConnectionString);

            using var scope = _factory.Services.CreateScope();

            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            db.Database.EnsureDeleted();
            db.Database.Migrate();

            Client = _factory.CreateClient();
        }

        public void Dispose()
        {
            Client.Dispose();
            _factory.Dispose();
        }
    }
}
