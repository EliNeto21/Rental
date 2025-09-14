using System.Text;
using Infra.Context;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text.Json;
using Domain.Event;

namespace Services.Consumers
{
    public class MotorcycleConsumer
    {
        private readonly AppDbContext _dbContext;
        private readonly string _queueName = "motorcycle-registered";

        public MotorcycleConsumer(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task StartAsync(CancellationToken cancellationToken = default)
        {
            var factory = new ConnectionFactory()
            {
                HostName = "localhost", 
                Port = 5672,
                UserName = "guest",
                Password = "guest"
            };

            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(
                queue: _queueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                try
                {
                    var moto = JsonSerializer.Deserialize<MotorcycleEvent>(message);

                    if (moto != null && moto.Year == 2024)
                    {
                        // salva no banco apenas se ano == 2024
                        await _dbContext.Motorcycles.AddAsync(new Domain.Entities.Motorcycle(
                            moto.Year,
                            moto.Model,
                            moto.Plate
                        ));

                        await _dbContext.SaveChangesAsync(cancellationToken);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[ERROR] Falha no consumer: {ex.Message}");
                }
            };

            channel.BasicConsume(
                queue: _queueName,
                autoAck: true,
                consumer: consumer
            );

            // mantém rodando até parar
            await Task.Delay(Timeout.Infinite, cancellationToken);
        }
    
    }

    public record MotorcycleMessage(Guid Id, int Year, string Model, string Plate);
}
