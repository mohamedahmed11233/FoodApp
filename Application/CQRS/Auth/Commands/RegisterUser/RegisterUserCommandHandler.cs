using Application.CQRS.Auth.Commend.RegisterUser;
using Application.CQRS.Auth.Queries.LoginUser;
using Application.Dtos.Auth;
using Application.Interfaces;
using Domain.Models;
using Infrastructure.IRepositories;
using MediatR;
using Microsoft.AspNetCore.Identity;


namespace Application.CQRS.Auth.Command.RegisterUser
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, RegisterResponseDto>
    {
        private readonly IGenericRepository<User> _userRepository;
        private readonly IJwtGenerator _jwtGenerator;
        private readonly IPasswordHasher<User> _passwordHasher;

        public RegisterUserCommandHandler(IGenericRepository<User> UserRepo, IJwtGenerator jwtGenerator, IPasswordHasher<User> passwordHasher)
        {
            _userRepository = UserRepo;
            _jwtGenerator = jwtGenerator;
            _passwordHasher = passwordHasher;
        }

        public async Task<RegisterResponseDto> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var existingUser = await _userRepository.GetBySpecAsync(u => u.Email == request.RegisterDto.Email);
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
                Username = request.RegisterDto.Email.Split('@')[0],
                Role = request.RegisterDto.Role
            };
            var existingUserByUsername = await _userRepository.GetBySpecAsync(
                u => u.Username.ToLower() == request.RegisterDto.Username.ToLower());

            if (existingUserByUsername != null)
            {
                return new RegisterResponseDto
                {
                    IsSucess = false,
                    Message = "Username is already taken"
                };
            }
                user.Password = _passwordHasher.HashPassword(user, request.RegisterDto.Password);


            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();

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
