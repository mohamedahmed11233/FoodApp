using Hangfire;
using Hotel_Reservation_System.Middleware;
using Infrastructure.service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Presentation.ExtensionMethods;
using Presentation.Helpers;
using System.Text;


namespace Presentation
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                  {
        options.TokenValidationParameters = new()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
                 });
            // Add services to the container.
            builder.Services.AddDependencyInjectionMethods(builder.Configuration);
            builder.Services.AddHostedService<BackgroundJobService>();
            builder.Services.AddHostedService<RabbitMQConsumerService>();
            builder.Services.AddHangfire(opt =>
            { opt.UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection") , new Hangfire.SqlServer.SqlServerStorageOptions
            {
                CommandTimeout = TimeSpan.FromMinutes(2),
                QueuePollInterval = TimeSpan.FromSeconds(15),
                PrepareSchemaIfNecessary = true,
            }); 
            });
            builder.Services.AddHangfireServer();    
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
