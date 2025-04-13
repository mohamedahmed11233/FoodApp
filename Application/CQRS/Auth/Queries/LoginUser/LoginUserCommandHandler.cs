using Application.Interfaces;
using Domain.Dtos.Auth;
using Domain.Models;
using Infrastructure.IRepositories;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.Auth.Queries.LoginUser
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, RegisterResponseDto>
    {
        private readonly IGenericRepository<User> _userRepository;
        private readonly IJwtGenerator _jwtGenerator;
        private readonly IPasswordHasher<User> _passwordHasher;

        public LoginUserCommandHandler(
            IGenericRepository<User> userRepository,
            IJwtGenerator jwtGenerator,
            IPasswordHasher<User> passwordHasher)
        {
            _userRepository = userRepository;
            _jwtGenerator = jwtGenerator;
            _passwordHasher = passwordHasher;
        }

        public async Task<RegisterResponseDto> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            // Fetch the user by email
            var user = await _userRepository.GetBySpecAsync(u => u.Email == request.Email && !u.IsDeleted);

            if (user == null)
                return new RegisterResponseDto
                {
                    IsSucess = false,
                    Message = "Invalid email or password"
                };

            // Verify the password
            var result = _passwordHasher.VerifyHashedPassword(user, user.Password, request.Password);

            if (result == PasswordVerificationResult.Failed)
                return new RegisterResponseDto
                {
                    IsSucess = false,
                    Message = "Invalid email or password"
                };

            var token = _jwtGenerator.GenerateToken(user);

            return new RegisterResponseDto
            {
                IsSucess = true,
                Message = "Login successful",
                Token = token,
                Username = user.Username
            };
        }
    }
}
