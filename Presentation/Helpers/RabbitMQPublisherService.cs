using RabbitMQ.Client;
using System.Text;

namespace Presentation.Helpers
{
    public class RabbitMQPublisherService
    {
        private readonly IConnection _connection;
        private readonly IChannel _channel;
        public RabbitMQPublisherService(IConnection connection, IChannel channel)
        {
            var factory = new ConnectionFactory { HostName = "localhost" };
            _connection = factory.CreateConnectionAsync().Result;
            _channel = _connection.CreateChannelAsync().Result;

            _channel.ExchangeDeclareAsync("newExchange", ExchangeType.Fanout, durable: true, autoDelete: false);
            _channel.QueueDeclareAsync("newQueue", durable: true, autoDelete: false);

            _channel.QueueBindAsync("newQueue", "newExchange" , "Noha");
        }
        public async Task PublishChannel(string message)
        {
           await _channel.BasicPublishAsync("newExchange", "Noha", Encoding.UTF8.GetBytes(message));
        }

    }
}
