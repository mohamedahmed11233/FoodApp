using Domain.Enum.SharedEnums;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Helper.Authorization
{
    public class FeatureRequirement : IAuthorizationRequirement
    {
        public FeatureEnum Feature { get; }

        public FeatureRequirement(FeatureEnum feature)
        {
            Feature = feature;
        }
    }
}
