using Application.CQRS.Auth.Queries.AssignFeatures;
using Domain.Enum.SharedEnums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Application.Helper.Authorization
{
    public class FeatureAuthorizationHandler : AuthorizationHandler<FeatureRequirement>
    {
        private readonly IMediator _mediator;
        private readonly IHttpContextAccessor _accessor;

        public FeatureAuthorizationHandler(IMediator mediator, IHttpContextAccessor accessor)
        {
            _mediator = mediator;
            _accessor = accessor;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, FeatureRequirement requirement)
        {
            var user = _accessor.HttpContext?.User;
            var roleValue = user?.FindFirst(ClaimTypes.Role)?.Value;

            if (Enum.TryParse<Roles>(roleValue, out var role))
            {
                var hasFeature = await _mediator.Send(new CheckRoleHasFeatureQuery(role, requirement.Feature));
                if (hasFeature)
                {
                    context.Succeed(requirement);
                }
            }
        }
    }
}
