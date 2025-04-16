using Domain.Enum.SharedEnums;
using Microsoft.AspNetCore.Authorization;

namespace Presentation.Helpers
{
    public class HasFeatureAttribute: AuthorizeAttribute
    {
        public HasFeatureAttribute(FeatureEnum feature)
        {
            Policy = $"Feature:{feature}";
        }
    }
}
