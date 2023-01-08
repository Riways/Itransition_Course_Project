using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using totten_romatoes.Server.Data;
using totten_romatoes.Shared;
using totten_romatoes.Shared.Models;

namespace totten_romatoes.Server.Services
{
    public interface IUserService
    {
        public Task<ApplicationUser> GetUserByUsername(string username);
        public Task<ApplicationUser> GetUserById(string id);
        public Task<List<ApplicationUser>> GetChunkOfUsers(int chunkNumber);
        public Task<int> GetAmountOfUsers();
        public Task<string> CreateUser(string username, string email, string password);
        public Task DeleteMuiltipleUsers(List<string> ids);
        public Task<bool> IsUsernameAvailable(string username);
    }
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _dbContext;
        private UserManager<ApplicationUser> _userManager { get; set; }
        private readonly IUserStore<ApplicationUser> _userStore;
        private PasswordHasher<ApplicationUser> hasher = new();

        public UserService(UserManager<ApplicationUser> userManager, ApplicationDbContext dbContext, IUserStore<ApplicationUser> userStore)
        {
            _userManager = userManager;
            _dbContext = dbContext;
            _userStore = userStore;
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
            List<ApplicationUser> users = await _dbContext.Users!
                .Skip(reviewsToSkip)
                .Take(Constants.USERS_ON_ADMIN_PAGE)
                .ToListAsync();
            return users;
        }

        public async Task<int> GetAmountOfUsers()
        {
            return await _dbContext.Users.CountAsync();
        }

        public async Task<string> CreateUser(string username, string email, string password)
        {
            ApplicationUser newUser = new()
            {
                UserName = username,
                NormalizedUserName = username.ToUpper(),
                Email = email,
                NormalizedEmail = email.ToUpper(),
                EmailConfirmed = true,
            };
            newUser.PasswordHash = hasher.HashPassword(newUser, password);
            await _userStore.CreateAsync(newUser, CancellationToken.None);
            return newUser.Id;
        }

        public async Task DeleteMuiltipleUsers(List<string> ids)
        {
            List<ApplicationUser> usersToDelete = await _dbContext.Users.Where(u => ids.Contains(u.Id)).ToListAsync();
            if (usersToDelete.Count > 0)
            {
                _dbContext.Users.RemoveRange(usersToDelete);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<bool> IsUsernameAvailable(string username)
        {
            return !await _dbContext.Users.AnyAsync(u => u.UserName.Equals(username));
        }
    }
}
