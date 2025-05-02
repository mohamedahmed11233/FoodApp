using Microsoft.EntityFrameworkCore.Metadata;
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
            _connection = connection;
            _channel = channel;

            _channel.ExchangeDeclareAsync("newExchange", ExchangeType.Fanout, durable: true, autoDelete: false);
            _channel.QueueDeclareAsync("newQueue", durable: true, exclusive: false, autoDelete: false);
            _channel.QueueBindAsync("newQueue", "newExchange", "Noha");
        }

        public void PublishMessage(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);
            _channel.BasicPublishAsync("newExchange", "Noha", false, body);
        }
    }

    
}
