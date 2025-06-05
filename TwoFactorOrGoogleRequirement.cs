using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Threading.Tasks;

namespace TwoFA
{
    

    public class TwoFactorOrGoogleRequirement : IAuthorizationRequirement { }

    public class TwoFactorOrGoogleHandler : AuthorizationHandler<TwoFactorOrGoogleRequirement>
    {
        private readonly UserManager<IdentityUser> _userManager;

        public TwoFactorOrGoogleHandler(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            TwoFactorOrGoogleRequirement requirement)
        {
            var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return; // Brak użytkownika
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return; // Brak użytkownika
            }

            var isTwoFactorEnabled = user.TwoFactorEnabled; // Sprawdza, czy 2FA jest aktywne
            var isGoogleLogin = context.User.HasClaim(c => c.Issuer == "Google");

            if (isTwoFactorEnabled || isGoogleLogin)
            {
                context.Succeed(requirement); // Spełniono wymogi dostępu
            }
        }
    }

}
