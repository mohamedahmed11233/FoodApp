using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.Auth.Commands.ForgetPassword
{
    public record RequestResetPasswordCommand(string Email):IRequest<bool>;
   
}
