using Application.CQRS.Auth.Queries.LoginUser;
using Application.Interfaces;
using Application.IRepositories;
using Domain.Dtos.Auth;
using Domain.Enum.SharedEnums;
using Domain.Models;
using Infrastructure.IRepositories;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.Auth.Commend.RegisterUser
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, AuthDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJwtGenerator _jwtGenerator;
        private readonly IPasswordHasher<User> _passwordHasher;

        public RegisterUserCommandHandler(IUnitOfWork unitOfWork, IJwtGenerator jwtGenerator, IPasswordHasher<User> passwordHasher)
        {
            _unitOfWork = unitOfWork;
            _jwtGenerator = jwtGenerator;
            _passwordHasher = passwordHasher;
        }

        public async Task<AuthDto> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var existingUser = await _unitOfWork.Repository<User>().GetBySpecAsync(u => u.Email == request.Email);
            if (existingUser != null)
            {
                return new AuthResponse(false, "User already exists with this email", null, null);
            }

            var user = new User
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Username = request.Username,
                Email = request.Email,
                Password = _passwordHasher.HashPassword(null, request.Password),
                IsDeleted = false,
            };

            await _unitOfWork.Repository<User>().AddAsync(user);
            await _unitOfWork.SaveChangesAsync();

            var token = _jwtGenerator.GenerateToken(user);

            return new AuthResponse(true, "User registered successfully", token, user.Username);
        }
    }
}
