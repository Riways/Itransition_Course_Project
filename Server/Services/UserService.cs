using Microsoft.AspNetCore.Identity;
using totten_romatoes.Server.Data;
using totten_romatoes.Shared.Models;

namespace totten_romatoes.Server.Services
{
    public interface IUserService
    {
        public Task<ApplicationUser> GetUserByUsername(string username);
        public Task<ApplicationUser> GetUserById(string id);
    }
    public class UserService : IUserService
    {
        private UserManager<ApplicationUser> _userManager { get; set; }

        public UserService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ApplicationUser> GetUserByUsername(string username)
        {
            ApplicationUser user = await _userManager.FindByNameAsync(username);
            return user;
        }

        public async Task<ApplicationUser> GetUserById(string id)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(id);
            return user;
        }
    }
}
