using Application.Dtos.Auth;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.Auth.Commands.OTP
{
    public record VerifyOtpCodeCommand(VerifyOtpRequestDto VerifyOtp) :IRequest<bool>;
   
}
