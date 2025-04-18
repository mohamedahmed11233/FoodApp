using Domain.Enum.SharedEnums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.Auth.Commend.AssignRoleToUser
{
    public class AssignRoleToUserCommand : IRequest<bool>
    {
        public int UserId { get; set; }
        public Roles Role { get; set; }

        public AssignRoleToUserCommand(int userId, Roles role)
        {
            UserId = userId;
            Role = role;
        }
    }
}
