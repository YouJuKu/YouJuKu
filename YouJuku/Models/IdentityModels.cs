using System.Security.Claims;
using System.Threading.Tasks;
using DHTMLX.Scheduler;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace YouJuku.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public override string UserName { get; set; }

        public override string Email { get; set; }

        [DHXJson(Alias = "first_name")]
        public string FirstName { get; set; }

        [DHXJson(Alias = "last_name")]
        public string LastName { get; set; }

        [DHXJson(Alias = "color")]
        public string Color { get; set; }

        
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }
}