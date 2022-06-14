// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VotingApp.Business.Constants;
using VotingApp.Data.Data;
using VotingApp.Data.Models;
using Microsoft.EntityFrameworkCore;
using VotingApp.Business.Interfaces;

namespace VotingApp.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _config;
        private readonly IHashingService _hashing;
        private readonly ISMSService _sms;
        private readonly IAuthenticationCodeService _code;
        private readonly INamingService _naming;

        public RegisterModel(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            ApplicationDbContext context,
            IConfiguration config,
            IHashingService hashing,
            ISMSService sms,
            IAuthenticationCodeService code,
            INamingService naming)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _config = config;
            _hashing = hashing;
            _sms = sms;
            _code = code;
            _naming = naming;
        }

        [BindProperty]
        public InputModel Input { get; set; }
        public string ReturnUrl { get; set; }
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "{0} е задължително.")]
            [Display(Name = "Име")]
            public string FirstName { get; set; }

            [Required(ErrorMessage = "{0} е задължително.")]
            [Display(Name = "Презиме")]
            public string MiddleName { get; set; }

            [Required(ErrorMessage = "{0} е задължителна.")]
            [Display(Name = "Фамилия")]
            public string LastName { get; set; }

            [Required]
            [StringLength(10, ErrorMessage = "{0} трябва да бъде с дължина {1} цифри.", MinimumLength = 10)]
            [DataType(DataType.Password)]
            [Display(Name = "ЕГН")]
            [RegularExpression("^[0-9]*$", ErrorMessage = "ЕГН-то трябва да се състои само от цифри!")]
            public string EGN { get; set; }

            [Required]
            [StringLength(9, ErrorMessage = "{0} трябва да е {1} цифри.", MinimumLength = 9)]
            [Display(Name = "Телефонен номер")]
            [RegularExpression("^[0-9]*$", ErrorMessage = "Телефонният номер трябва да се състои само от цифри!")]
            public string PhoneNumber { get; set; }

        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = CreateUser();
                var _password = _code.CreateAuthenticationCode();

                if (await IsPhoneNumberUsed(user.PhoneNumber))
                {
                    ModelState.AddModelError(string.Empty, "Телефонният номер вече съществува в системата!");
                }
                else
                {
                    var result = await _userManager.CreateAsync(user, _password);
                    await _userManager.AddToRoleAsync(user, Roles.User);
                    _sms.SendMessage(_password, Input.PhoneNumber);

                    if (result.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }

            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

        private User CreateUser()
        {
            try
            {
                var newUser = new User()
                {
                    FirstName = _naming.UppercaseFirst(Input.FirstName),
                    MiddleName = _naming.UppercaseFirst(Input.MiddleName),
                    LastName = _naming.UppercaseFirst(Input.LastName),
                    PhoneNumber = "359" + Input.PhoneNumber,
                    UserName = _hashing.CustomHashing(Input.EGN),
                };

                return newUser;
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(User)}'. ");
            }
        }

        private async Task<bool> IsPhoneNumberUsed(string phone)
        {
            var phoneToLookFor = await _context.Users
                .FirstOrDefaultAsync(x => x.PhoneNumber == phone);

            if (phoneToLookFor == null)
            {
                return false;
            }
            return true;

        }
    }
}
