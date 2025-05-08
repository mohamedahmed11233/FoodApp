using MediatR;
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
        private readonly IMediator _mediator;
        public RabbitMQConsumerService(IMediator mediator)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            _connection = factory.CreateConnectionAsync().Result;
            _channel = _connection.CreateChannelAsync().Result;
            _mediator = mediator;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            var Consumer = new AsyncEventingBasicConsumer(_channel);
            Consumer.ReceivedAsync += Consumer_ReceivedAsync;
            _channel.BasicConsumeAsync("newQueue",false,Consumer);
            return Task.CompletedTask;
        }

        private  async Task Consumer_ReceivedAsync(object sender, BasicDeliverEventArgs @event)
        {
            try
            {
                var body = Encoding.UTF8.GetString(@event.Body.ToArray());
                var message = GetMessage(body);

                await InvokeConsumer(message);
                await _channel.BasicAckAsync(@event.DeliveryTag, false);
            }
            catch (Exception ex) when (ex is NullReferenceException) 
            {
                await _channel.BasicRejectAsync(@event.DeliveryTag, true);
            }
            catch(Exception)     
            {
                await _channel.BasicRejectAsync(@event.DeliveryTag, false);
            }
        }

        private Task InvokeConsumer(BaseMessage message)
        {
            message.Type =  message.Type.Replace("Message",  "Consumer");
            var NameSpace = "Presentation.ViewModel.RabbitMQ";
            var type = Type.GetType($"{NameSpace}.{message.Type},Presentation");
            if (type is null) throw new NullReferenceException("Type not found");
            var Consumer = Activator.CreateInstance(type , _mediator);
            var method  = type.GetMethod("ConsumeAsync");
            if (method is null) throw new NullReferenceException("Method not found");
           
            method.Invoke(Consumer, new object[] { message });
            return Task.CompletedTask;
        }

        private BaseMessage GetMessage(string body)
        {
           var JsonObject= Newtonsoft.Json.Linq.JObject.Parse(body);
           var TypeName = JsonObject["Type"].ToString();
           var Namespace = "Presentation.ViewModel.RabbitMQ";
           var type = Type.GetType($"{Namespace}.{TypeName},Presentation");
            if (type is null) throw new NullReferenceException("Type not found");
            
            var baseMEssage = Newtonsoft.Json.JsonConvert.DeserializeObject(body, type) as BaseMessage;
            if (baseMEssage is null)
            {
                throw new NullReferenceException("BaseMessage not found");
            }
            return baseMEssage;

        }

        public async Task StopAsync(CancellationToken cancellationToken = default)
        {
            await _channel.CloseAsync();
            await _connection.CloseAsync();
        }
    }
}
