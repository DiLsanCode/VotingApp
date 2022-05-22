using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using VotingApp.Business.Interfaces;
using VotingApp.Business.Models;

namespace VotingApp.Controllers
{
    public class UserController : Controller
    {
        //private readonly SignInManager<RegisterModel> _signInManager;
        private readonly IUserService _userService;

        //SignInManager<RegisterModel> signInManager,
        public UserController( IUserService userService)
        {
            //_signInManager = signInManager;
            _userService = userService;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel user)
        {
            if (ModelState.IsValid)
            {
                await _userService.CreateNewUser(user);
                //await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
    }
}
