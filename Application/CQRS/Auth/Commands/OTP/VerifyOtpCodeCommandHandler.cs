using Domain.Models;
using Infrastructure.IRepositories;
using MediatR;
using OtpNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.Auth.Commands.OTP
{
    public class VerifyOtpCodeCommandHandler : IRequestHandler<VerifyOtpCodeCommand, bool>
    {
        private readonly IGenericRepository<User> _userrepository;

        public VerifyOtpCodeCommandHandler(IGenericRepository<User> userrepository) 
        {
            _userrepository = userrepository;
        }
        public async Task<bool> Handle(VerifyOtpCodeCommand request, CancellationToken cancellationToken)
        {
            var user =await _userrepository.GetByIdAsync(request.VerifyOtp.UserId);
            if (user is null || string.IsNullOrEmpty(user.OtpSekretKey))
                return false;

            var secretKeyBytes = Base32Encoding.ToBytes(user.OtpSekretKey);
            var totp = new Totp(secretKeyBytes);
            var isValid = totp.VerifyTotp(request.VerifyOtp.Code, out long timeStepMatched, VerificationWindow.RfcSpecifiedNetworkDelay);
            return isValid;

        }
    }
}
