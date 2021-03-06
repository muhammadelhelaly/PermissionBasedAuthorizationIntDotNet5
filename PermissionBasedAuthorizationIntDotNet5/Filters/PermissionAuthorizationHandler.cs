using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Threading.Tasks;

namespace PermissionBasedAuthorizationIntDotNet5.Filters
{
    public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
    {
        public PermissionAuthorizationHandler()
        {

        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            if (context.User == null)
                return;

            var canAccess = context.User.Claims.Any(c => c.Type == "Permission" && c.Value == requirement.Permission && c.Issuer == "LOCAL AUTHORITY");

            if (canAccess)
            {
                context.Succeed(requirement);
                return;
            }
        }
    }
}