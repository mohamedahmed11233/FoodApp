using Application.CQRS.Auth.Command.RegisterUser;
using Application.CQRS.Auth.Commend.RegisterUser;
using Application.Dtos.Auth;
using Application.Interfaces;
using Application.IRepositories;
using Application.Repositories;
using Application.service;
using Domain.Models;
using Hotel_Reservation_System.Error;
using Hotel_Reservation_System.Middleware;
using Infrastructure.Context;
using Infrastructure.IRepositories;
using Infrastructure.Repositories;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Presentation.ExtensionMethods
{
    public static class AddExtensionMethods
    {
        public static IServiceCollection AddDependencyInjectionMethods(this IServiceCollection Services , IConfiguration configuration)
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
                    typeof(Program).Assembly,  // إضافة معالجات المشروع الحالي
                    Assembly.Load("Application")  // إضافة معالجات مشروع "Application"
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
            Services.AddAutoMapper(typeof(MappingProfile.Mapping));
            Services.AddScoped<IRequestHandler<RegisterUserCommand, RegisterResponseDto>, RegisterUserCommandHandler>();
            Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
            Services.AddScoped<IGenericRepository<User>, GenericRepository<User>>();


            Services.AddScoped<IJwtGenerator, JwtGenerator>();
            return Services;
        }
    }
}
