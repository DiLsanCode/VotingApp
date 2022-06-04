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
using VotingApp.Business.Validation;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Text;

namespace VotingApp.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IUserStore<User> _userStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _config;

        public RegisterModel(
            UserManager<User> userManager,
            IUserStore<User> userStore,
            SignInManager<User> signInManager,
            ILogger<RegisterModel> logger,
            ApplicationDbContext context,
            IConfiguration config)
        {
            _userManager = userManager;
            _userStore = userStore;
            _signInManager = signInManager;
            _logger = logger;
            _context = context;
            _config = config;
        }

        [BindProperty]
        public InputModel Input { get; set; }
        public string ReturnUrl { get; set; }
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required]
            [Display(Name = "Име")]
            public string FirstName { get; set; }

            [Required]
            [Display(Name = "Презиме")]
            public string MiddleName { get; set; }

            [Required]
            [Display(Name = "Фамилия")]
            public string LastName { get; set; }

            [Required]
            [StringLength(10, ErrorMessage = "{0} трябва да бъде с дължина {1} цифри.", MinimumLength = 10)]
            [DataType(DataType.Password)]
            [Display(Name = "ЕГН")]
            [RegularExpression("^[0-9]*$", ErrorMessage = "ЕГН-то трябва да се състои само от цифри!")]
            public string EGN { get; set; }

            [Required]
            [StringLength(9, ErrorMessage = "{0} трябва да е {1}.", MinimumLength = 9)]
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
                var _password = CreateAuthenticationCode();

                if (await IsPhoneNumberUsed(user.PhoneNumber))
                {
                    ModelState.AddModelError(string.Empty, "Телефонният номер вече съществува в системата!");
                }
                else
                {
                    var result = await _userManager.CreateAsync(user, _password);
                    await _userManager.AddToRoleAsync(user, Roles.User);

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
                    FirstName = UppercaseFirst(Input.FirstName),
                    MiddleName = UppercaseFirst(Input.MiddleName),
                    LastName = UppercaseFirst(Input.LastName),
                    PhoneNumber = "+359" + Input.PhoneNumber,
                    UserName = CustomHashing(Input.EGN),
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

        private string UppercaseFirst(string str)
        {
            if (string.IsNullOrEmpty(str))
                return string.Empty;
            return char.ToUpper(str[0]) + str.Substring(1).ToLower();
        }

        private string CreateAuthenticationCode()
        {
            Random rnd = new Random();
            const string chars = "AaBbCcDdEeFfGgHhIiJjKkkLlMmNnOoPpQqRrSsTtUuVvWwXxYyZz0123456789";
            return new string(Enumerable.Repeat(chars, 8)
                .Select(s => s[rnd.Next(s.Length)]).ToArray());
        }

        private string CustomHashing(string str)
        {
            var key = _config.GetValue<string>("SecurityKey:Key");
            byte[] salt = Encoding.ASCII.GetBytes(key);

            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: str,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 1,
                numBytesRequested: 64));

            return hashed;
        }

    }
}
