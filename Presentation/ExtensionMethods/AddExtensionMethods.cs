using Application.CQRS.Auth.Command.RegisterUser;
using Application.CQRS.Auth.Commend.RegisterUser;
using Application.Dtos.Auth;
using Application.Helper.Authorization;
using Application.Interfaces;
using Application.IRepositories;
using Application.Repositories;
using Application.service;
using Domain.Enum.SharedEnums;
using Domain.Models;
using Hangfire;
using Hotel_Reservation_System.Error;
using Hotel_Reservation_System.Middleware;
using Infrastructure.Context;
using Infrastructure.IRepositories;
using Infrastructure.Repositories;
using Infrastructure.service;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Presentation.Helpers;
using RabbitMQ.Client;
using System.Reflection;
using System.Text;
using System.Threading.Channels;

namespace Presentation.ExtensionMethods
{
    public static class AddExtensionMethods
    {
        public static IServiceCollection AddDependencyInjectionMethods(this IServiceCollection Services ,IConfiguration configuration)
        {
            Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            Services.AddEndpointsApiExplorer();
            Services.AddSwaggerGen();
            Services.AddScoped<GlobalTransactionMiddleware>();
            Services.AddDbContext<FoodAppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
            Services.AddScoped<IUnitOfWork, UnitOfWork>();
            Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            Services.AddMediatR(cfg =>
                cfg.RegisterServicesFromAssemblies(
                    typeof(Program).Assembly,  
                    Assembly.Load("Application")  
                )
            );
            #region ApiValidationErrorr
            Services.Configure<ApiBehaviorOptions>(opthion =>
            {
                opthion.InvalidModelStateResponseFactory = (actionContext) =>
                {
                    var errors = actionContext.ModelState.Where(o => o.Value?.Errors.Count() > 0)
                                                         .SelectMany(o => o.Value.Errors)
                                                         .Select(e => e.ErrorMessage)
                                                         .ToList();
                    var response = new ApiValidationError()
                    {
                        Errors = errors
                    };
                    return new BadRequestObjectResult(response);
                };
            });
            #endregion
            Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            

            Services.AddScoped<IRequestHandler<RegisterUserCommand, RegisterResponseDto>, RegisterUserCommandHandler>();
            Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
            Services.AddScoped<IGenericRepository<User>, GenericRepository<User>>();
            Services.AddSingleton<RabbitMQPublisherService>();
            Services.AddSingleton<IConnection>(sp =>
            {
                var factory = new ConnectionFactory { HostName = "localhost" }; // Adjust settings as needed
                return factory.CreateConnectionAsync().GetAwaiter().GetResult();
            });
         
            Services.AddSingleton<IChannel>(sp =>
            {
                var connection = sp.GetRequiredService<IConnection>();
                return connection.CreateChannelAsync().GetAwaiter().GetResult();
            });
            Services.AddHttpContextAccessor();

            Services.AddTransient<FeatureAuthorizationHandler>();
            Services.AddAuthorization();
            Services.AddControllersWithViews();
            Services.AddStackExchangeRedisCache(opt =>
            { opt.Configuration = "redis-19177.c16.us-east-1-3.ec2.redns.redis-cloud.com,Password = ycAxPeersm2FneXOC69bW5mzMZwAFdol";
              opt.InstanceName = "memory";
            });
            Services.AddCap(options =>
            {
                options.UseEntityFramework<FoodAppDbContext>();
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
                options.UseRabbitMQ(rabbitMQ =>
                {
                    rabbitMQ.HostName = "localhost";
                    rabbitMQ.UserName = "guest";
                    rabbitMQ.Password = "guest";
                    rabbitMQ.Port = 15672;
                    rabbitMQ.ExchangeName = "cap.default.router";

                });
            });
            Services.AddAuthorization(options =>
            {
                foreach (var feature in Enum.GetValues(typeof(FeatureEnum)))
                {
                    options.AddPolicy($"Feature:{feature}", policy =>
                    {
                        policy.Requirements.Add(new FeatureRequirement((FeatureEnum)feature));
                    });
                }
            });


            Services.AddScoped<IAuthorizationHandler, FeatureAuthorizationHandler>();

            Services.AddScoped<IJwtGenerator, JwtGenerator>();
            Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme )
              .AddJwtBearer(options =>
              {
                  options.TokenValidationParameters = new()
                  {
                      ValidateIssuer = true,
                      ValidateAudience = true,
                      ValidateLifetime = true,
                      ValidateIssuerSigningKey = true,
                      ValidIssuer = configuration["Jwt:Issuer"],
                      ValidAudience = configuration["Jwt:Audience"],
                      IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!))
                  };
              });
            Services.AddMemoryCache();
            Services.AddHostedService<BackgroundJobService>();
            Services.AddHostedService<RabbitMQConsumerService>();
            Services.AddHangfire(opt =>
            {
                opt.UseSqlServerStorage(configuration.GetConnectionString("DefaultConnection"), new Hangfire.SqlServer.SqlServerStorageOptions
                {
                    CommandTimeout = TimeSpan.FromMinutes(2),
                    QueuePollInterval = TimeSpan.FromSeconds(15),
                    PrepareSchemaIfNecessary = true,
                });
            });
            Services.AddHangfireServer();
            return Services;
        }
    }
}
