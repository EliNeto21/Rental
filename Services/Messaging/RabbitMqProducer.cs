using RabbitMQ.Client;
using System.Text.Json;
using System.Text;
using Services.IMessaging;
using RabbitMQ.Client.Events;

namespace Services.Messaging
{
    public class RabbitMqProducer : IRabbitMqProducer
    {
        private readonly string _hostname = "localhost";

        public async Task PublishAsync<T>(string queue, T message)
        {
            var factory = new ConnectionFactory { HostName = _hostname };

            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(
                queue: queue,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );

            var consumer = new EventingBasicConsumer(channel);

            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

            channel.BasicConsume(
                queue: queue,
                autoAck: true,
                consumer: consumer
            );
        }
    }
}
