using Domain.Enum.SharedEnums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.Auth.Queries.AssignFeatures
{
    public class CheckRoleHasFeatureQuery : IRequest<bool>
    {
        public Roles Role { get; set; }
        public FeatureEnum Feature { get; set; }

        public CheckRoleHasFeatureQuery(Roles role, FeatureEnum feature)
        {
            Role = role;
            Feature = feature;
        }
    }
}
