using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace TwoFA.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Example for configuring custom fields if needed
            builder.Entity<ApplicationUser>(entity =>
            {
                entity.Property(e => e.TwoFactorSecret).HasMaxLength(10000).IsRequired(false);
                entity.Property(e => e.GoogleAuthenticatorKey).HasMaxLength(10000).IsRequired(false);
            });
        }
    }


}
