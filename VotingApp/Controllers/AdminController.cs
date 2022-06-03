using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VotingApp.Business.Constants;
using VotingApp.Business.Interfaces;
using VotingApp.Business.Models;
using VotingApp.Data.Data;
using VotingApp.Data.Models;

namespace VotingApp.Controllers
{
    [Authorize(Roles = Roles.Admin)]
    public class AdminController : Controller
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpGet]
        public IActionResult AddNewParticipant() { return View(); }

        [HttpGet]
        public IActionResult AddNewParty() { return View(); }

        [HttpGet]
        public IActionResult ConfirmDelete() { return View(); }

        /*[HttpGet]
        public IActionResult EditParticipant() { return View(); }*/

        [HttpPost]
        public async Task<IActionResult> AddNewParticipant(RegisterNewParticipant newParticipant)
        {
            if (ModelState.IsValid)
            {
                await _adminService.AddParticipant(newParticipant);
                return RedirectToAction("ListParticipants","User");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddNewParty(RegisterNewParty newParty)
        {
            if (ModelState.IsValid)
            {
                await _adminService.AddParty(newParty);
                return RedirectToAction("ListParticipants", "User");
            }
            return View();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteParticipant(int id)
        {
            if (ModelState.IsValid)
            {
                await _adminService.DeleteParticipant(id);
                return RedirectToAction("ListParticipants", "User");
            }
            return View();
        }

        [HttpPut]
        public async Task<IActionResult> EditParticipant()
        {
            return View();
        }
    }
}
