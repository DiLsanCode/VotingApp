using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VotingApp.Data.Data;
using VotingApp.Data.Models;

namespace VotingApp.Controllers
{
    public class UserController : Controller
    {
        private readonly RoleManager<Role> _roleManager;
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _context;

        public UserController(RoleManager<Role> roleManager, 
                                UserManager<User> userManager, 
                                ApplicationDbContext dbContext)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _context = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> ListParticipants()
        {
            var participants = await _context.Participants.ToListAsync();
            return View(participants);
        }
    }
}
