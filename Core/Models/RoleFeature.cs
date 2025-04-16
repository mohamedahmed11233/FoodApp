using Domain.Enum.SharedEnums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class RoleFeature:BaseEntity
    {
        public Roles Role { get; set; }
        public FeatureEnum Feature { get; set; }
    }
}
