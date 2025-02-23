
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Data;
using WorkSphere.Data;
using WorkSphere.Server.Dtos;
using WorkSphere.Server.Model;

namespace WorkSphere.Server.Repository
{
    /// <inheritdoc />
    public class LoginRepo : ILoginRepo
    {
        private readonly WorkSphereDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public LoginRepo(
            WorkSphereDbContext context,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }


        public async Task<UserDto?> LoginAsync(LoginDto model)
        {
            if (model == null)
            {
                return null;
            }
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return null;

            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
            if (!result.Succeeded)
            {
                return null;
            }

            // Get user's role
            var roles = await _userManager.GetRolesAsync(user);
            string userRole = roles.FirstOrDefault() ?? "User"; // Default to "User" if no role is found

            // Fetch the role name from AspNetRoles table
            var roleName = await _context.Roles
                .Where(r => r.Name == userRole)
                .Select(r => r.Name)
                .FirstOrDefaultAsync();

            var userDto = new UserDto
            {
                Username = user.UserName,
                Email = user.Email,
                Role = roleName ?? "User"
            };



            return userDto;

        }

    }
}
