using EventRegistrationSystem.Areas.Identity.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace EventRegistrationSystem.Extensions
{
    public static class UserManagerExtensions
    {
        public static async Task<bool> IsInRoleAsync<T>(this UserManager<T> userManager, ClaimsPrincipal principal, string role) where T : ApplicationUser
        {
            var user = await userManager.GetUserAsync(principal);
            return user != null && await userManager.IsInRoleAsync(user, role);
        }
    }
}
