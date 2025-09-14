using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Infrastructure.Messaging
{
    public class RabbitMqConsumer
    {
        private readonly string _hostname = "localhost";
        private readonly string _queueName = "motorcycles";

        public async Task StartConsumingAsync()
        {
            var factory = new ConnectionFactory() { HostName = _hostname };

            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(
                queue: _queueName,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                Console.WriteLine($"Mensagem recebida: {message}");
            };

            channel.BasicConsume(
                queue: _queueName,
                autoAck: true,
                consumer: consumer
            );

            Console.WriteLine("Consumer iniciado. Pressione [enter] para sair.");
            Console.ReadLine();
        }
    }
}
