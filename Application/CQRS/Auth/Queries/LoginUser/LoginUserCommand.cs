using Domain.Dtos.Auth;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.Auth.Queries.LoginUser
{
    public record LoginUserCommand(
     string Email,
     string Password
   ): IRequest<RegisterResponseDto>;

}
