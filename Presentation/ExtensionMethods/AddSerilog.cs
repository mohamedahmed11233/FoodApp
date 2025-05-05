using Serilog;

namespace Presentation.ExtensionMethods
{
    public static class AddSerilog
    {
        public static ILoggingBuilder AddSerilogLogger(this ILoggingBuilder Logging, IConfiguration configuration , WebApplicationBuilder builder)
        {
            Logging.AddConsole();
            Logging.AddDebug();
            Logging.ClearProviders();

            Serilog.Log.Logger = new LoggerConfiguration().WriteTo.Seq("http://localhost:5341")
                .Enrich.WithEnvironmentName()
                .Enrich.WithMachineName()
               .WriteTo.MSSqlServer
            (
              connectionString:configuration.GetConnectionString("DefaultConnection") ,
              restrictedToMinimumLevel:Serilog.Events.LogEventLevel.Information,
              sinkOptions:new Serilog.Sinks.MSSqlServer.MSSqlServerSinkOptions { AutoCreateSqlTable =true,TableName="Logs"  } 

            ).CreateLogger();

            builder.Host.UseSerilog();
            return Logging;
           
           
        }
    }
}
