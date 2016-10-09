using System.Collections.Generic;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using YouJuku.Models;

namespace YouJuku.Migrations
{
    using System.Data.Entity.Migrations;
    internal sealed class Configuration : DbMigrationsConfiguration<Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "YouJuku.Models.ApplicationDbContext";
        }

        protected override void Seed(ApplicationDbContext context)
        {
            //This method will be called after migrating to the latest version.
            var roles = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            foreach (var role in new List<string> {"Admin", "User"})
            {
                if (!roles.RoleExists(role))
                {
                    roles.Create(new IdentityRole(role));
                }
            }

            var users = new List<ApplicationUser>()
            {
                new ApplicationUser
                {
                    UserName = "shimodayoshiko1001@hotmail.com",
                    Email = "shimodayoshiko1001@hotmail.com",
                    FirstName = "Yoshiko",
                    LastName = "Kim",
                    Color = "#666666"
                },
                new ApplicationUser
                {
                    UserName = "tonkymm@yahoo.com",
                    Email = "tonkymm@yahoo.com",
                    FirstName = "Tony",
                    LastName = "Kim",
                    Color = ""
                }
            };

            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            foreach (var user in users)
            {
                var result = userManager.Create(user, "123456");
                if (result != null && result.Succeeded)
                {
                    userManager.AddToRole(user.Id, "Admin");
                }
            }
        }
    }
}
