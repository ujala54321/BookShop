
using BookShoppingCartMartUI.Contants;
using BookShoppingCartMartUI.Migrations;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace BookShoppingCartMartUI.Data
{
    public class DbSeeder
    {
        public static async Task SeedDefaultData(IServiceProvider service)
        {
            var userMgr = service.GetService<UserManager<IdentityUser>>();
            var roleMgr = service.GetService<RoleManager<IdentityRole>>();
            /*  await roleMgr.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
              await roleMgr.CreateAsync(new IdentityRole(Roles.User.ToString()));
              var admin = new IdentityUser
              {
                  UserName = "admin@gmail.com",
                  Email = "admin@gmail.com",
                  EmailConfirmed = true
              };
              var userInDb = await userMgr.FindByEmailAsync(admin.Email.ToLower());
              if (userInDb is null)
              {
                  await userMgr.CreateAsync(admin, "Admin@123");
                  await userMgr.AddToRoleAsync(admin, Roles.Admin.ToString());
              }
          }*/
            if (!await roleMgr.RoleExistsAsync(Roles.Admin.ToString()))
                await roleMgr.CreateAsync(new IdentityRole(Roles.Admin.ToString()));

            if (!await roleMgr.RoleExistsAsync(Roles.User.ToString()))
                await roleMgr.CreateAsync(new IdentityRole(Roles.User.ToString()));

            var adminEmail = "admin@gmail.com";
            var adminUser = await userMgr.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                var admin = new IdentityUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                var result = await userMgr.CreateAsync(admin, "Admin@123");
                if (result.Succeeded)
                {
                    await userMgr.AddToRoleAsync(admin, Roles.Admin.ToString());
                }
            }
            else
            {
                // ✅ If admin already exists, make sure they’re in Admin role
                if (!await userMgr.IsInRoleAsync(adminUser, Roles.Admin.ToString()))
                    await userMgr.AddToRoleAsync(adminUser, Roles.Admin.ToString());
            }

        }
    }
}
