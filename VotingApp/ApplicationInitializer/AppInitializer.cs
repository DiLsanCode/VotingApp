using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VotingApp.Business.Constants;
using VotingApp.Data.Data;
using VotingApp.Data.Models;

namespace VotingApp.ApplicationInitializer
{
    public class AppInitializer
    {
        private readonly ApplicationDbContext _context;
        private UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;

        public AppInitializer(ApplicationDbContext context, UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task InitializeAsync()
        {
            await ApplyMigrationsAsync();
            await SeedRolesAsync();
            await SeedUsersAsync();
        }

        private async Task ApplyMigrationsAsync()
        {
            var pendingMigrations = await _context.Database.GetPendingMigrationsAsync();

            if (pendingMigrations.Any())
            {
                await _context.Database.MigrateAsync();
            }
        }

        private async Task SeedRolesAsync()
        {
            if (await _context.Roles.AnyAsync())
            {
                return;
            }

            await _roleManager.CreateAsync(new Role { Name = Roles.Admin });
            await _roleManager.CreateAsync(new Role { Name = Roles.User });
        }

        private async Task SeedUsersAsync()
        {
            if (await _context.Users.AnyAsync())
            {
                return;
            }

            var adminUser = new User
            {
                UserName = "Admin",
                EGN = "0000000000",
                FirstName = "Admin",
                MiddleName = "Admin",
                LastName = "Admin",
            };

            await _userManager.CreateAsync(adminUser, "Admin123!");
            await _userManager.AddToRoleAsync(adminUser, Roles.Admin);

        }
    }
}
