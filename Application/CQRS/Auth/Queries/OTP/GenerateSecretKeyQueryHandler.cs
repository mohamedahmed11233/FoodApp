using Application.Dtos.Auth;
using Domain.Models;
using Infrastructure.IRepositories;
using MediatR;
using OtpNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.Auth.Queries.OTP
{
    public class GenerateSecretKeyQueryHandler : IRequestHandler<GenerateSecretKeyQuery, GenerateSecretKeyDTO>
    {
        private readonly IGenericRepository<User> _userRepository;

        public GenerateSecretKeyQueryHandler(IGenericRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<GenerateSecretKeyDTO> Handle(GenerateSecretKeyQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId);
            if (user == null)
                throw new Exception("User not found.");

            if (!string.IsNullOrEmpty(user.OtpSekretKey))
            {
                return new GenerateSecretKeyDTO
                {
                    SecretKey = user.OtpSekretKey,
                    AuthenticatorUri = GenerateUri(user.Username, user.OtpSekretKey)
                };
            }

            var key = KeyGeneration.GenerateRandomKey(20);
            var base32Secret = Base32Encoding.ToString(key);

            user.OtpSekretKey = base32Secret;

            await _userRepository.UpdateInclude(user, nameof(user.OtpSekretKey));

            return new GenerateSecretKeyDTO
            {
                SecretKey = base32Secret,
                AuthenticatorUri = GenerateUri(user.Username, base32Secret)
            };
        }


        private string GenerateUri(string userName, string secretKey)
        {
            string appName = "FoodApp"; 
            return $"otpauth://totp/{appName}:{userName}?secret={secretKey}&issuer={appName}";
        }


    }
}
