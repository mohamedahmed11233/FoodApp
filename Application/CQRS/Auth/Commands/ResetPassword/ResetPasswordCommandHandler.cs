using Domain.Models;
using Infrastructure.IRepositories;
using MediatR;
using Microsoft.AspNetCore.Identity;
using OtpNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.Auth.Commands.ResetPassword
{
    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, bool>
    {
        private readonly IGenericRepository<User> _userRepository;
        private readonly IPasswordHasher<User> _passwordHasher;

        public ResetPasswordCommandHandler(IGenericRepository<User> userRepository, IPasswordHasher<User> passwordHasher)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }
        public async Task<bool> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.resetPassword.UserId);
          

            if (user.ResetCode != request.resetPassword.OtpCode)
                return false;
         
            // Change Password
            user.Password = _passwordHasher.HashPassword(user, request.resetPassword.NewPassword);
            await _userRepository.UpdateInclude(user, nameof(user.Password));

            user.ResetCode = null;
            await _userRepository.UpdateInclude(user, nameof(user.ResetCode));

            return true;
        }
    }
}
