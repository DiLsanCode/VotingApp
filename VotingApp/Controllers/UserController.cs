using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VotingApp.Business.Constants;
using VotingApp.Business.Models;
using VotingApp.Data.Data;
using VotingApp.Data.Models;

namespace VotingApp.Controllers
{
    [Authorize(Roles = Roles.User)]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext dbContext)
        {
            _context = dbContext;
        }

        
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ListParticipants()
        {
            var participants = await _context.Participants
                .Include(x => x.Party)
                .Select(x => new ViewParticipantInParty
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Party = x.Party.Name
                })
                .ToListAsync();
            return View(participants);
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmVote(int id)
        {
            var participant = await _context.Participants
                .Select(x => new ViewParticipantInParty
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Party = x.Party.Name
                })
                .FirstOrDefaultAsync(x => x.Id == id);

            return View(participant);
        }

        [HttpPut]
        public async Task<IActionResult> Vote(int id)
        {
            var participant = await _context.Participants
                .FirstOrDefaultAsync(x => x.Id == id);

            if(participant != null)
            {
                participant.VoteCount++;
                var updatedOrder = _context.Participants.Update(participant);
                await _context.SaveChangesAsync();
                return RedirectToAction("ListParticipants", "User");
            }
            return View();
        }
    }
}
