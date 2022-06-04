using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VotingApp.Business.Constants;
using VotingApp.Business.Interfaces;
using VotingApp.Business.Models;
using VotingApp.Data.Data;
using VotingApp.Data.Models;

namespace VotingApp.Controllers
{
    [Authorize(Roles = Roles.User)]
    public class UserController : Controller
    {
        private readonly IUserService _userSerive;

        public UserController(IUserService userSerive)
        {
            _userSerive = userSerive;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ListOfAllParticipants()
        {
            var participants = await _userSerive.GetAllListOfParticipant();
            return View(participants);
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmVote(int id)
        {
            var participant = await _userSerive.GetParticipantById(id);
            return View(participant);
        }

        [HttpPost]
        public async Task<IActionResult> Vote(int id, int userId)
        {
            if (ModelState.IsValid)
            {
                await _userSerive.Vote(id, userId);
                return RedirectToAction("ListOfAllParticipants", "User");
            }
            return View();
        }
    }
}
