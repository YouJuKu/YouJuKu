using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data.Entity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace YouJuku.Models
{
    public class SchedulerDbInitializer : DropCreateDatabaseIfModelChanges<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            var users = new UserManager<ApplicationUser>(new
                    UserStore<ApplicationUser>(context)); 
 
            var roles = new RoleManager<IdentityRole>(new
                                    RoleStore<IdentityRole>(context));
 
            string name = "Admin@mail.com";
            string password = "123456";
  
      
            // Create Admin role
            var adminRole = "Admin";
            if (!roles.RoleExists(adminRole))
            {
                var roleresult = roles.Create(new IdentityRole(adminRole));
            }
      
            // Create account for the admin
            var user = new ApplicationUser
            {
                UserName = name,
                Email = name
            };
         
            var adminresult = users.Create(user, password);
      
            // Assign new user to admin role
            if (adminresult.Succeeded)
            {
                var result = users.AddToRole(user.Id, adminRole);
            }
 
            // Create several test users
            var test_users = new string[]{
                "Alex@mail.com", 
                "John@mail.com", 
                "Sarah@mail.com", 
                "Paul@mail.com"
            };
 
            foreach (var username in test_users)
            {
                users.Create(new ApplicationUser { 
                    UserName = username, 
                    Email = username 
                }, "123456");
            }
             
            base.Seed(context);
        }
    }
}