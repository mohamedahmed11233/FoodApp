using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.Helpers;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HangfireController : ControllerBase
    {
        [HttpGet("TestFireAndForget")]
        public Task<ResponseViewModel<string>> FireAndForget()
        {
            // Run once 
            BackgroundJob.Enqueue(() => Console.WriteLine("Hello from 1st test !!"));
            return Task.FromResult(new ResponseViewModel<string>(true, Domain.Enum.SharedEnums.ErrorCode.NotFound, null, "Job Enqueued"));

        }

        [HttpGet("TestDalyedJob")]
        public Task<ResponseViewModel<string>> DelayedJob()
        {
            // Run once 
            BackgroundJob.Schedule(() => Console.WriteLine("Hello from 1st test !!"), TimeSpan.FromSeconds(30));
            return Task.FromResult(new ResponseViewModel<string>(true, Domain.Enum.SharedEnums.ErrorCode.NotFound, null, "Job Schedulued"));

        }
        [HttpGet("RecurringJob")]
        public Task<ResponseViewModel<string>> RecurringJob_Test()
        {
            // Run once 
            RecurringJob.AddOrUpdate("test", () => Console.WriteLine("Hello from 1st test !!"), Cron.Minutely);
            return Task.FromResult(new ResponseViewModel<string>(true, Domain.Enum.SharedEnums.ErrorCode.NotFound, null, "Job Enqueued"));

        }
    }
}
