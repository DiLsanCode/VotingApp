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
        public IActionResult AddNewParticipant()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddNewParticipant(RegisterNewParticipant newParticipant)
        {
            if (ModelState.IsValid)
            {
                await _adminService.AddParticipant(newParticipant);
                return RedirectToAction("Index","Home");
            }
            return View();
        }
    }
}
