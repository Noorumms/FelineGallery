using Microsoft.AspNetCore.Authorization;
using Feline_Gallery_v1.Authorization.Requirement;

namespace Feline_Gallery_v1.Authorization.Handler
{
    public class PermissionAuthorizationRequirementHandler : AuthorizationHandler<PermissionAuthorizationRequirement>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            PermissionAuthorizationRequirement requirement)
        {
            // Check if user has the required permission claim
            if (context.User.HasClaim(c => c.Type == CustomClaimTypes.Permission &&
                                          c.Value == requirement.Permission))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;

        }
    }
}

