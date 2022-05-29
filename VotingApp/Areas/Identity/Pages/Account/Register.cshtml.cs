// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using VotingApp.Business.Constants;
using VotingApp.Data.Data;
using VotingApp.Data.Models;

namespace VotingApp.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IUserStore<User> _userStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly ApplicationDbContext _context;

        public RegisterModel(
            UserManager<User> userManager,
            IUserStore<User> userStore,
            SignInManager<User> signInManager,
            ILogger<RegisterModel> logger,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _userStore = userStore;
            _signInManager = signInManager;
            _logger = logger;
            _context = context;
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
            [StringLength(10, ErrorMessage = "The {0} must be {1} characters long.", MinimumLength = 10)]
            [DataType(DataType.Password)]
            [Display(Name = "ЕГН")]
            public string EGN { get; set; }

            [Required]
            [StringLength(12, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 9)]
            [Display(Name = "Телефонен номер")]
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

                await _userStore.SetUserNameAsync(user, Input.EGN, CancellationToken.None);
                var result = await _userManager.CreateAsync(user, _password);
                await _userManager.AddToRoleAsync(user, Roles.User);

                if (result.Succeeded)
                {
                    _logger.LogInformation("Успешна регистрация!");

                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return LocalRedirect(returnUrl);
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
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
                    PhoneNumber = Input.PhoneNumber,
                    UserName = Input.EGN
                };

                return newUser;
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(User)}'. ");
            }
        }

        private async Task<string> PhoneNumberAsync(string str)
        {
            var phoneToLookFor = await _context.Users
               .FirstOrDefaultAsync(x => x.PhoneNumber == str);

            if(phoneToLookFor != null)
            {
                
            }
            return str;
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

        /*private string CustomHashing(string str)
        {
            // generate a 128-bit salt using a cryptographically strong random sequence of nonzero values
            byte[] salt = new byte[128 / 8];
            *//*using (var rngCsp = new RNGCryptoServiceProvider())
            {
                rngCsp.GetNonZeroBytes(salt);
            }*//*

            // derive a 256-bit subkey (use HMACSHA256 with 1,000 iterations)
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: str,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 1000,
                numBytesRequested: 256 / 8));

            return hashed;
        }*/
    }
}
