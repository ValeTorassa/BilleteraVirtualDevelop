using AutheticationAPI2._0.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace AutheticationAPI2._0.Data
{
    public class SeedManager
    {

        public static async Task Seed(IServiceProvider services)
        {
            await SeedRoles(services);
            await SeedAdminUser(services);
        }

        private static async Task SeedRoles(IServiceProvider services)
        {
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

            await roleManager.CreateAsync(new IdentityRole(StaticUserRoles.Admin));
            await roleManager.CreateAsync(new IdentityRole(StaticUserRoles.User));
        }

        private static async Task SeedAdminUser(IServiceProvider services)
        {
            var context = services.GetRequiredService<AppDbContext>();
            var userManager = services.GetRequiredService<UserManager<User>>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

            var adminUser = await context.Users.FirstOrDefaultAsync(user => user.UserName == "AuthenticationAdmin");


            if (adminUser is null)
            {
                adminUser = new User { UserName = "Admin", Email = "valen@gmail.com" };
                await userManager.CreateAsync(adminUser, "Valen123*");
                await userManager.AddToRoleAsync(adminUser, StaticUserRoles.Admin);
            }
        }
    }
}
