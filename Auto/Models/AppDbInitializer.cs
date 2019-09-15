using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Auto.Models
{
    public class AppDbInitializer : CreateDatabaseIfNotExists<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(context));

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
                        
            var role1 = new IdentityRole { Name = "admin" };
            var role2 = new IdentityRole { Name = "user" };

            roleManager.Create(role1);
            roleManager.Create(role2);
                        
            var admin = new ApplicationUser { Email = "nagula.anton@mail.ru", UserName = "nagula.anton@mail.ru" };
            string password = "1234abcd";
            var result = userManager.Create(admin, password);
            
            if (result.Succeeded)
            {
                userManager.AddToRole(admin.Id, role1.Name);             
            }

            var user = new ApplicationUser { Email = "some@mail.ru", UserName = "some@mail.ru" };
            var res = userManager.Create(user, password);
            
            if (res.Succeeded)
            {                
                userManager.AddToRole(user.Id, role2.Name);
            }

            var user2 = new ApplicationUser { Email = "any@mail.ru", UserName = "any@mail.ru" };
            var res2 = userManager.Create(user2, password);

            if (res2.Succeeded)
            {
                userManager.AddToRole(user2.Id, role2.Name);
            }
            base.Seed(context);
        }
    }
}