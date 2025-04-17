using Application.Dtos.User;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.Auth.Queries.UserDataOperation
{
    public record GetAllUserQuery() :IRequest<IEnumerable<UserDto>>;
    
}
