using System.Text;
using RabbitMQ.Client;

namespace Infrastructure.Messaging
{
    public class RabbitMqPublisher
    {
        private readonly string _hostname = "localhost";
        private readonly string _queueName = "motorcycles";

        public void Publish(string message)
        {
            var factory = new ConnectionFactory() { HostName = _hostname };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(queue: _queueName,
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(exchange: "",
                                 routingKey: _queueName,
                                 basicProperties: null,
                                 body: body);
        }
    }
}
