using Presentation.ViewModel.RabbitMQ;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Presentation.Helpers
{
    public class RabbitMQConsumerService : IHostedService
    {
        private readonly IChannel _channel;
        private readonly IConnection _connection;

        public RabbitMQConsumerService()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            _connection = factory.CreateConnectionAsync().Result;
            _channel = _connection.CreateChannelAsync().Result;

        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            var Consumer = new AsyncEventingBasicConsumer(_channel);
            Consumer.ReceivedAsync += Consumer_ReceivedAsync;
            _channel.BasicConsumeAsync("newQueue", false , Consumer);
            return Task.CompletedTask;
        }

        private  async Task Consumer_ReceivedAsync(object sender, BasicDeliverEventArgs @event)
        {
            try
            {
                var body = Encoding.UTF8.GetString(@event.Body.ToArray());
                var message = GetMessage(body);
                await _channel.BasicAckAsync(@event.DeliveryTag, false);
            }
            catch (Exception ex) when (ex is NullReferenceException) 
            {
                await _channel.BasicRejectAsync(@event.DeliveryTag, true);
            }
            catch(Exception ex)
            {
                await _channel.BasicRejectAsync(@event.DeliveryTag, false);
            }
        }

        private BaseMessage GetMessage(string body)
        {
           var JsonObject= Newtonsoft.Json.Linq.JObject.Parse(body);
           var TypeName = JsonObject["Type"].ToString();
           var Namespace = "Presentation.ViewModel.RabbitMQ";
           Type type = Type.GetType($"{Namespace}.{TypeName},Presentation");
            if (type is null) throw new NullReferenceException("Type not found");
            
            var baseMEssage = Newtonsoft.Json.JsonConvert.DeserializeObject(body, type) as BaseMessage;
            if (baseMEssage is null)
            {
                throw new NullReferenceException("BaseMessage not found");
            }
            return baseMEssage;

        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _channel.CloseAsync();
            await _connection.CloseAsync();
        }
    }
}
