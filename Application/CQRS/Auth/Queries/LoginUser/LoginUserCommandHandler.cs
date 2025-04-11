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
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, AuthResponse>
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

        public async Task<AuthResponse> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetBySpecAsync(u => u.Email == request.Email && !u.IsDeleted);

            if (user == null)
                return new AuthResponse(false, "Invalid email or password", null, null);

            var result = _passwordHasher.VerifyHashedPassword(user, user.Password, request.Password);

            if (result == PasswordVerificationResult.Failed)
                return new AuthResponse(false, "Invalid email or password", null, null);

            var token = _jwtGenerator.GenerateToken(user);

            return new AuthResponse
            {
                IsSuccess = false,
                Message = "Invalid email or password",
                Token = null,
                Username = null,
                IsAuthenticated = false,
                Email = "",
                Roles = null,
                ExpiresOn = DateTime.MinValue
            };
        }
    }
}
