using EventRegistrationSystem.Areas.Identity.Models;
using Microsoft.AspNetCore.Identity;

namespace EventRegistrationSystem.Extensions
{
    public static class SignInManagerExtensions
    {
        public static async Task<SignInResult> EmailPasswordSignInAsync<T>(this SignInManager<T> signInManager, string email, string password, bool isPersistent, bool lockoutOnFailure) where T : ApplicationUser
        {
            var user = await signInManager.UserManager.FindByEmailAsync(email);
            if (user == null)
            {
                return SignInResult.Failed;
            }

            return await signInManager.PasswordSignInAsync(user, password, isPersistent, lockoutOnFailure);
        }
    }
}
