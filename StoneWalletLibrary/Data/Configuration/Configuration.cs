using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Migrations;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace StoneWalletLibrary.Data.Configuration
{
    class Configuration:DbMigrationsConfiguration<StoneWalletContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = false;
        }

        //protected override void Seed(StoneWalletContext context)
        //{
        //    string adminRoleId;
        //    string userRoleId;
        //    if (!context.Roles.Any())
        //    {
        //        adminRoleId = context.Roles.Add(new IdentityRole("Administrator")).Id;
        //        userRoleId = context.Roles.Add(new IdentityRole("User")).Id;
        //    }
        //    else
        //    {
        //        adminRoleId = context.Roles.First(c => c.Name == "Administrator").Id;
        //        userRoleId = context.Roles.First(c => c.Name == "User").Id;
        //    }

        //    context.SaveChanges();

        //    if (!context.Users.Any())
        //    {
        //        var administrator = context.Users.Add(new IdentityUser("administrator") { Email = "admin@somesite.com", EmailConfirmed = true });
        //        administrator.Roles.Add(new IdentityUserRole { RoleId = adminRoleId });

        //        var standardUser = context.Users.Add(new IdentityUser("jonpreece") { Email = "jon@somesite.com", EmailConfirmed = true });
        //        standardUser.Roles.Add(new IdentityUserRole { RoleId = userRoleId });

        //        context.SaveChanges();

        //        var store = new StoneWalletUserStore();
        //        store.SetPasswordHashAsync(administrator, new UserManager<ApplicationUser, int>(new UserStore<ApplicationUser, ApplicationRole, int, ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>(context)).PasswordHasher.HashPassword("administrator123"));
        //        store.SetPasswordHashAsync(standardUser, new UserManager().PasswordHasher.HashPassword("user123"));
        //    }

        //    context.SaveChanges();
        //}
    }
}
