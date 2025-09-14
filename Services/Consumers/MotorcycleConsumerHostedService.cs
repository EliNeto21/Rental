using Infra.Context;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Services.Consumers
{
    public class MotorcycleConsumerHostedService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public MotorcycleConsumerHostedService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var scope = _serviceProvider.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var consumer = new MotorcycleConsumer(db);
            await consumer.StartAsync(stoppingToken);
        }
    }
}
