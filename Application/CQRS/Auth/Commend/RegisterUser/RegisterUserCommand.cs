using Domain.Dtos.Auth;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.Auth.Commend.RegisterUser
{
    public record RegisterUserCommand(
     string FirstName,
     string LastName,
     string Username,
     string Email,
     string Password
 ) : IRequest<AuthDto>;

}
