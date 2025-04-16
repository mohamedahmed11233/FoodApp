using Domain.Enum.SharedEnums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.Auth.Commend.AssignFeatures
{
    public record AssignRoleFeaturesCommand:IRequest<bool> 
    {
        public Roles Role { get; set; }
        public List<FeatureEnum> Features { get; set; }
    }
}
