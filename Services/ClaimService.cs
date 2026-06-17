using Microsoft.AspNetCore.Identity;
using Feline_Gallery_v1.Models;
using System.Security.Claims;

namespace Feline_Gallery_v1.Services
{
    public class ClaimsService : IClaimsService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ClaimsService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task AddPermissionClaimAsync(ApplicationUser user, string permission)
        {
            var claim = new Claim(Authorization.CustomClaimTypes.Permission, permission);
            await _userManager.AddClaimAsync(user, claim);
        }

        public async Task RemovePermissionClaimAsync(ApplicationUser user, string permission)
        {
            var claims = await _userManager.GetClaimsAsync(user);
            var claim = claims.FirstOrDefault(c =>
                c.Type == Authorization.CustomClaimTypes.Permission &&
                c.Value == permission);

            if (claim != null)
            {
                await _userManager.RemoveClaimAsync(user, claim);
            }
        }

        public async Task<IList<Claim>> GetUserPermissionsAsync(ApplicationUser user)
        {
            var claims = await _userManager.GetClaimsAsync(user);
            return claims.Where(c => c.Type == Authorization.CustomClaimTypes.Permission).ToList();
        }

        public async Task AssignAdminClaimsAsync(ApplicationUser user)
        {
            var adminPermissions = new[]
            {
                Authorization.Permissions.AccessAdminDashboard,
                Authorization.Permissions.ViewArtworks,
                Authorization.Permissions.CreateArtworks,
                Authorization.Permissions.EditArtworks,
                Authorization.Permissions.DeleteArtworks,
                Authorization.Permissions.ViewCategories,
                Authorization.Permissions.CreateCategories,
                Authorization.Permissions.EditCategories,
                Authorization.Permissions.DeleteCategories,
                Authorization.Permissions.ViewArtists,
                Authorization.Permissions.CreateArtists,
                Authorization.Permissions.EditArtists,
                Authorization.Permissions.DeleteArtists,
                Authorization.Permissions.ViewExhibitions,
                Authorization.Permissions.CreateExhibitions,
                Authorization.Permissions.EditExhibitions,
                Authorization.Permissions.DeleteExhibitions,
                Authorization.Permissions.ViewOrders,
                Authorization.Permissions.ManageOrders
            };

            foreach (var permission in adminPermissions)
            {
                await AddPermissionClaimAsync(user, permission);
            }
        }

        public async Task AssignCustomerClaimsAsync(ApplicationUser user)
        {
            var customerPermissions = new[]
            {
                Authorization.Permissions.ViewOwnOrders
            };

            foreach (var permission in customerPermissions)
            {
                await AddPermissionClaimAsync(user, permission);
            }
        }
    }
}