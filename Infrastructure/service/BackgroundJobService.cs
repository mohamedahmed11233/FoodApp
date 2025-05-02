using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.service
{
    public class BackgroundJobService : IHostedService
    {
        public Task StartAsync(CancellationToken cancellationToken)
        {
            //while (!cancellationToken.IsCancellationRequested)
            //{
            //    // Your background job logic here
            //    // For example, you can call a method to process jobs from a queue
            //    // await ProcessJobsAsync(cancellationToken);

            //    // Simulate some work
            //    Task.Delay(1000, cancellationToken).Wait(cancellationToken);
            //    Thread.Sleep(1000);
            //}
            return Task.CompletedTask;

        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
