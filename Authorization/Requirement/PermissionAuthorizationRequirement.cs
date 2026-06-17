using Microsoft.AspNetCore.Authorization;

namespace Feline_Gallery_v1.Authorization.Requirement
{

    public class PermissionAuthorizationRequirement : IAuthorizationRequirement
    {
        public string Permission { get; }

        public PermissionAuthorizationRequirement(string permission)
        {
            Permission = permission;
        }
    }
}
