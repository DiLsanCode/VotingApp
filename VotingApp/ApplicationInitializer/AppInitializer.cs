using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text;
using VotingApp.Business.Constants;
using VotingApp.Data.Data;
using VotingApp.Data.Models;

namespace VotingApp.ApplicationInitializer
{
    public class AppInitializer
    {
        private readonly ApplicationDbContext _dbContext;
        private UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IConfiguration _config;

        public AppInitializer(ApplicationDbContext dbContext, UserManager<User> userManager, RoleManager<Role> roleManager, IConfiguration config)
        {
            _dbContext = dbContext;
            _roleManager = roleManager;
            _userManager = userManager;
            _config = config;
        }

        public async Task InitializeAsync()
        {
            await ApplyMigrationsAsync();
            await SeedRolesAsync();
            await SeedUsersAsync();
            await SeedPartiesAsync();
        }

        private async Task ApplyMigrationsAsync()
        {
            var pendingMigrations = await _dbContext.Database.GetPendingMigrationsAsync();

            if (pendingMigrations.Any())
            {
                await _dbContext.Database.MigrateAsync();
            }
        }

        private async Task SeedRolesAsync()
        {
            if (await _dbContext.Roles.AnyAsync())
            {
                return;
            }

            await _roleManager.CreateAsync(new Role { Name = Roles.Admin });
            await _roleManager.CreateAsync(new Role { Name = Roles.User });
        }

        private async Task SeedPartiesAsync()
        {
            if (await _dbContext.Users.AnyAsync())
            {
                return;
            }

            for(int i = 1; i < 7; i++)
            {
                await _dbContext.AddAsync(new Party()
                {
                    Name = $"Party {i}",
                });
            }
            await _dbContext.SaveChangesAsync();
        }

        private async Task SeedUsersAsync()
        {
            if (await _dbContext.Users.AnyAsync())
            {
                return;
            }

            var adminUser = new User
            {
                UserName = CustomHashing("Admin12345"),
                FirstName = "Admin",
                MiddleName = "Admin",
                LastName = "Admin",
            };

            await _userManager.CreateAsync(adminUser, "Admin123");
            await _userManager.AddToRoleAsync(adminUser, Roles.Admin);

        }

        private string CustomHashing(string str)
        {
            var key = _config.GetValue<string>("SecurityKey:Key");
            byte[] salt = Encoding.ASCII.GetBytes(key);

            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: str,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 1,
                numBytesRequested: 64));

            return hashed;
        }
    }
}
