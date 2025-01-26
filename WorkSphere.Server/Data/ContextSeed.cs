using Microsoft.AspNetCore.Identity;
using WorkSphere.Server.Enums;
using WorkSphere.Server.Model;

namespace WorkSphere.Server.Enums
{
    public enum Roles
    {
        Admin,
        ProjectManager,
        Employee
    }
}


namespace WorkSphere.Server.Data
{
    public class ContextSeed
    {
        public static async Task SeedDataAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            //Seed Roles
            await roleManager.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.ProjectManager.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.Employee.ToString()));
        }
    }
}
