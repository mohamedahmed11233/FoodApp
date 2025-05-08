using DotNetCore.CAP;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client.Events;
using System.Text;

namespace Presentation.Helpers.CAP_Library
{
    public class CAPConsumer : ICapSubscribe
    {
        [CapSubscribe("cap")]
        public Task Consume(string message)
        {
            return Task.CompletedTask;
            
           
        }
    }
}
