using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using StoneWalletLibrary.Models;

namespace StoneWalletLibrary.Data
{

    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class StoneWalletContext : IdentityDbContext<ApplicationUser>
    {
        public StoneWalletContext() : base("StoneWalletContext")
        {
              
        }
        public DbSet<Wallet> Wallets { get; set; }
        public DbSet<Card> Cards { get; set; }
        public DbSet<Cardholder> Cardholders { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Cardholder>().HasOptional(c => c.Wallet);
            modelBuilder.Entity<Wallet>().HasRequired(w => w.Cardholder);
        }
    }
}
