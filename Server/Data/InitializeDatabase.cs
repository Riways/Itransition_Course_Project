using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using totten_romatoes.Shared.Models;

namespace totten_romatoes.Server.Data
{
    public class InitializeDatabase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IConfiguration _appConfig;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserStore<ApplicationUser> _userStore;

        public InitializeDatabase(ApplicationDbContext dbContext, IConfiguration appConfig, UserManager<ApplicationUser> userManager, IUserStore<ApplicationUser> userStore)
        {
            _dbContext = dbContext;
            _appConfig = appConfig;
            _userManager = userManager;
            _userStore = userStore;
        }

        public async void Run()
        {
            await AddAdminRole();
            await AddAdminUser();
        }

        private async Task AddAdminRole()
        {
            if (!_dbContext.Roles.Any(r => r.Name == "Admin"))
            {
                RoleStore<IdentityRole> roleStore = new(_dbContext);
                IdentityRole adminRole = new()
                {
                    Name = "Admin",
                    NormalizedName = "Admin".ToUpper()
                };
                await roleStore.CreateAsync(adminRole);
            }
        }

        private async Task AddAdminUser()
        {
            if (!_dbContext.Users.Any(u => u.UserName == _appConfig["admin_username"]))
            {
                PasswordHasher<ApplicationUser> hasher = new();
                ApplicationUser administrator = new()
                {
                    UserName = _appConfig["admin_username"],
                    NormalizedUserName = _appConfig["admin_username"].ToUpper(),
                    Email = _appConfig["admin_email"],
                    NormalizedEmail = _appConfig["admin_email"].ToUpper(),
                    EmailConfirmed = true,
                };
                administrator.PasswordHash = hasher.HashPassword(administrator, _appConfig["admin_password"]);
                await _userStore.CreateAsync(administrator, CancellationToken.None);
                await _userManager.AddToRoleAsync(administrator, "ADMIN");
            }
        }
    }
}
