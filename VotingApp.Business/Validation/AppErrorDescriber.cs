using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VotingApp.Data.Data;

namespace VotingApp.Business.Validation
{
    public class AppErrorDescriber : IdentityErrorDescriber
    {
        private readonly ApplicationDbContext _context;

        public AppErrorDescriber(ApplicationDbContext context)
        {
            _context = context;
        }

        public override IdentityError DuplicateUserName(string userName)
        {
            var error = base.DuplicateUserName(userName);
            error.Description = "ЕГН-то вече съществува в системата!";
            return error;
        }

        public async Task<IdentityError> DuplicatePhoneNumbereAsync(string phoneNumber)
        {
            var phoneToLookFor = await _context.Users
                .FirstOrDefaultAsync(x => x.PhoneNumber == phoneNumber);

            var error = new IdentityError
            {
                Code = nameof(DuplicatePhoneNumbereAsync),
                Description = "Телефонният номер вече съществува в системата!"
            };

            if (phoneToLookFor != null)
            {
                return error;
            }

            return null;
        }
    }
}
