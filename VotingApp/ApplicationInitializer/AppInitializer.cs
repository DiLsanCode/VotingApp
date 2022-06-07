using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text;
using VotingApp.Business.Constants;
using VotingApp.Business.Interfaces;
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
        private readonly IHashingService _hashing;

        public AppInitializer(ApplicationDbContext dbContext, 
                                UserManager<User> userManager, 
                                RoleManager<Role> roleManager, 
                                IConfiguration config,
                                IHashingService hashing)
        {
            _dbContext = dbContext;
            _roleManager = roleManager;
            _userManager = userManager;
            _config = config;
            _hashing = hashing;
        }

        public async Task InitializeAsync()
        {
            await ApplyMigrationsAsync();
            await SeedRolesAsync();
            await SeedUsersAsync();
            await SeedPartiesAsync();
            await SeedParticipantsAsync();
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
            if (await _dbContext.Parties.AnyAsync())
            {
                return;
            }

            for(int i = 1; i < 7; i++)
            {
                await _dbContext.Parties.AddAsync(new Party()
                {
                    Name = $"Party {i}",
                });
            }
            await _dbContext.SaveChangesAsync();
        }

        private async Task SeedParticipantsAsync()
        {
            if (await _dbContext.Participants.AnyAsync())
            {
                return;
            }

            for (int i = 1; i < 7; i++)
            {
                await _dbContext.Participants.AddAsync(new Participant()
                {
                    FirstName = $"Participant {i}",
                    MIddleName = $"MiddleName {i}",
                    LastName = $"LastName {i}",
                    PartyId = i,
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
                UserName = _hashing.CustomHashing("Admin12345"),
                FirstName = "Admin",
                MiddleName = "Admin",
                LastName = "Admin",
            };

            await _userManager.CreateAsync(adminUser, "Admin123");
            await _userManager.AddToRoleAsync(adminUser, Roles.Admin);

        }
    }
}
