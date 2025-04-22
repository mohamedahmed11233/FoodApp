using Application.Interfaces;
using Domain.Models;
using Infrastructure.IRepositories;
using MediatR;
using System;
using System.Threading.Tasks;

namespace Application.CQRS.Auth.Commands.ForgetPassword
{
    public class RequestResetPasswordCommandHandler : IRequestHandler<RequestResetPasswordCommand, bool>
    {
        private readonly IGenericRepository<User> _userRepository;
        private readonly IEmailService _emailService;

        public RequestResetPasswordCommandHandler(IGenericRepository<User> userRepository, IEmailService emailService)
        {
            _userRepository = userRepository;
            _emailService = emailService;
        }

        public async Task<bool> Handle(RequestResetPasswordCommand request, CancellationToken cancellationToken)
        {
            // Find user by email
            var user = await _userRepository.GetBySpecAsync(u => u.Email == request.Email);
            if (user == null)
                return false;

            var resetCode = new Random().Next(100000, 999999).ToString(); 

            user.ResetCode = resetCode;  // Assuming `ResetCode` is a property of `User`
            await _userRepository.UpdateInclude(user, nameof(user.ResetCode));

            await _userRepository.SaveChangesAsync();
            // Create the email message
            var subject = "Password Reset Request";
            var message = $"Hello {user.FirstName},<br/>" +
                          $"We received a request to reset your password. Your reset code is: <strong>{resetCode}</strong><br/>" +
                          "Please use this code to reset your password. The code expires in 10 minutes.";

            try
            {
                await _emailService.SendEmailAsync(user.Email, subject, message);
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
    }
}
