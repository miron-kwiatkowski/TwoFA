using Microsoft.AspNetCore.Identity;

namespace TwoFA.Data
{
    public class ApplicationUser : IdentityUser
    {
        // Klucz do weryfikacji dwustopniowej
        public string TwoFactorSecret { get; set; }

        // Flaga oznaczająca, czy użytkownik ma włączone 2FA
        public bool IsTwoFactorEnabled { get; set; }

        // Dodatkowe pola na potrzeby logowania zewnętrznego (np. Google)
        public string GoogleAuthenticatorKey { get; set; }
    }
}