using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using totten_romatoes.Server.Data;
using totten_romatoes.Shared.Models;
using totten_romatoes.Shared;
using Microsoft.EntityFrameworkCore;

namespace totten_romatoes.Server.Services
{
    public interface IUserService
    {
        public Task<ApplicationUser> GetUserByUsername(string username);
        public Task<ApplicationUser> GetUserById(string id);
        public Task<List<ApplicationUser>> GetChunkOfUsers(int chunkNumber);
        public Task<int> GetAmountOfUsers();
        public Task DeleteMuiltipleUsers(List<string> ids);
    }
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _dbContext;
        private UserManager<ApplicationUser> _userManager { get; set; }

        public UserService(UserManager<ApplicationUser> userManager, ApplicationDbContext dbContext)
        {
            _userManager = userManager;
            _dbContext = dbContext;
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

        public async Task<List<ApplicationUser>> GetChunkOfUsers(int chunkNumber)
        {
            int reviewsToSkip = Constants.USERS_ON_ADMIN_PAGE * chunkNumber;
            var users = await _dbContext.Users!
                .Skip(reviewsToSkip)
                .Take(Constants.USERS_ON_ADMIN_PAGE)
                .ToListAsync();
            return users;
        }

        public async Task<int> GetAmountOfUsers()
        {
            return await _dbContext.Users.CountAsync();
        }

        public async Task DeleteMuiltipleUsers(List<string> ids)
        {
            List<ApplicationUser> usersToDelete = await _dbContext.Users.Where(u => ids.Contains(u.Id)).ToListAsync();
            if(usersToDelete.Count > 0)
            {
                _dbContext.Users.RemoveRange(usersToDelete);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
