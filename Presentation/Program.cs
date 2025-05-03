using Hotel_Reservation_System.Middleware;
using Presentation.ExtensionMethods;
using Serilog;
using Serilog.Events;
using System.Runtime.CompilerServices;

namespace Presentation
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            // Add services to the container.
            builder.Services.AddDependencyInjectionMethods(builder.Configuration);
            builder.Logging.AddSerilogLogger(builder.Configuration,builder);
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseMiddleware<ExceptionMiddleware>();
            app.UseMiddleware<GlobalTransactionMiddleware>();
            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }

        
    }
}
