using Feline_Gallery_v1.Models;
using System.Security.Claims;

namespace Feline_Gallery_v1.Services
{
    public interface IClaimsService
    {
        Task AddPermissionClaimAsync(ApplicationUser user, string permission);
        Task RemovePermissionClaimAsync(ApplicationUser user, string permission);
        Task<IList<Claim>> GetUserPermissionsAsync(ApplicationUser user);
        Task AssignAdminClaimsAsync(ApplicationUser user);
        Task AssignCustomerClaimsAsync(ApplicationUser user);
    }
}