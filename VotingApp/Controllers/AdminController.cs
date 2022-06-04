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

        //Show Page
        [HttpGet]
        public async Task<IActionResult> AddNewParticipant() 
        {
            var pl = await _adminService.GetListOfParties();
            ViewBag.ListOfParties = pl;
            return View(); 
        }

        [HttpGet]
        public IActionResult AddNewParty() { return View(); }

        [HttpGet]
        public async Task<IActionResult> EditParticipant(int id) 
        {
            var pl = await _adminService.GetListOfParties();
            ViewBag.ListOfParties = pl;
            var participant = await _adminService.GetParticipantForEdit(id);
            return View(participant);
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmDelete(int id)
        {
            var participant = await _adminService.GetParticipant(id);
            return View(participant);
        }


        [HttpPost]
        public async Task<IActionResult> AddNewParticipant(RegisterNewParticipant newParticipant)
        {
            var pl = await _adminService.GetListOfParties();
            ViewBag.ListOfParties = pl;

            if (ModelState.IsValid)
            {
                await _adminService.AddParticipant(newParticipant);
                return RedirectToAction("ListOfAllParticipants", "User");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddNewParty(RegisterNewParty newParty)
        {
            if (ModelState.IsValid)
            {
                await _adminService.AddParty(newParty);
                return RedirectToAction("ListOfAllParticipants", "User");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteParticipant(int id)
        {
            if (ModelState.IsValid)
            {
                await _adminService.DeleteParticipant(id);
                return RedirectToAction("ListOfAllParticipants", "User");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> EditParticipant(int id, RegisterNewParticipant participant)
        {
            var pl = await _adminService.GetListOfParties();
            ViewBag.ListOfParties = pl;

            if (ModelState.IsValid)
            {
                await _adminService.EditParticipant(id, participant);
                return RedirectToAction("ListOfAllParticipants", "User");
            }
            return View();
        }
    }
}
