using E_Commerce.Data.Models.Security;
using E_Commerce.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace E_Commerce.Services.Implementation
{
    public class UserService : IUserService
    {
        private readonly UserManager<UserSet> _userManager;
        private readonly RoleManager<RoleSet> _roleManager;

        public UserService(UserManager<UserSet> userManager, RoleManager<RoleSet> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<UserSet> FindByEmailAsync(string email) => await _userManager.FindByEmailAsync(email);

        public async Task AddUserAsync(UserSet user, string password) => await _userManager.CreateAsync(user, password);

        public async Task<bool> CheckPasswordAsync(UserSet user, string password) => await _userManager.CheckPasswordAsync(user, password);

        public async Task AddUserToRoleAsync(UserSet user, string role)
        {
            if (!await _roleManager.RoleExistsAsync(role))
            {
                await _roleManager.CreateAsync(new RoleSet { Name = role });
            }
            await _userManager.AddToRoleAsync(user, role);
        }
    }
}
