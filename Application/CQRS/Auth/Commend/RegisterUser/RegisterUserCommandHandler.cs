using Application.CQRS.Auth.Commend.RegisterUser;
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

namespace Application.CQRS.Auth.Command.RegisterUser
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, RegisterResponseDto>
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

        public async Task<RegisterResponseDto> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var existingUser = await _unitOfWork.Repository<User>().GetBySpecAsync(u => u.Email == request.RegisterDto.Email);
            if (existingUser != null)
            {
                return new RegisterResponseDto
                {
                    IsSucess = false,
                    Message = "User already exists with this email"
                };
            }

            var user = new User
            {
                FirstName = request.RegisterDto.FirstName,
                LastName = request.RegisterDto.LastName,
                Email = request.RegisterDto.Email,
                Username = request.RegisterDto.Username,
                Role = request.RegisterDto.Role
            };

            user.Password = _passwordHasher.HashPassword(user, request.RegisterDto.Password);


            await _unitOfWork.Repository<User>().AddAsync(user);
            await _unitOfWork.SaveChangesAsync();

            var token = _jwtGenerator.GenerateToken(user);

            return new RegisterResponseDto
            {
                IsSucess = true,
                Token = token,
                Username = request.RegisterDto.Username
            };
        }
    }
}
