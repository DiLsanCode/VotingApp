using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VotingApp.Business.Interfaces;
using VotingApp.Business.Models;
using VotingApp.Data.Data;
using VotingApp.Data.Models;

namespace VotingApp.Business.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public UserService(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<string> CreateNewUser(RegisterModel user)
        {
            await Validation(user);
            var _userName = UserNameToLower(user);

            var newUser = new User()
            {
                FirstName = UppercaseFirst(user.FirstName),
                MiddleName = UppercaseFirst(user.MiddleName),
                LastName = UppercaseFirst(user.LastName),
                PhoneNumber = user.PhoneNumber,
                EGN = CustomHashing(user.EGN),
                UserName = _userName,
            };

            await _userManager.CreateAsync(newUser);

            /*var IsCreatedResult = await _userManager.CreateAsync(newUser, user.EGN);

            if (!IsCreatedResult.Succeeded)
            {
                return "User was not added. Problem with EGN";
            }*/

            /*var AddRoleResult = await _userManager.AddToRoleAsync(newUser, user.Role.ToString());

            if (!AddRoleResult.Succeeded)
            {
                await _userManager.DeleteAsync(newUser);
                return "User was not added. Problem with role";
            }*/

            return "User added successfully";
        }
        
        private string CustomHashing(string str)
        {
            var _hashCode = 7;
            var result = (str.GetHashCode() * _hashCode).ToString();
            return result;
        }

        private string UppercaseFirst(string str)
        {
            if (string.IsNullOrEmpty(str))
                return string.Empty;
            return char.ToUpper(str[0]) + str.Substring(1).ToLower();
        }

        private string UserNameToLower(RegisterModel user)
        {
            var firstName = user.FirstName.ToLower();
            var middleName = user.MiddleName.ToLower();
            var lastName = user.LastName.ToLower();

            var _userName = firstName + "_" + middleName + "_" + lastName;

            return _userName;
        }

        private async Task Validation(RegisterModel user)
        {
            var egnToLookFor = await _context.Users
                .FirstOrDefaultAsync(x => x.EGN == user.EGN);

            var phoneToLookFor = await _context.Users
                .FirstOrDefaultAsync(x => x.PhoneNumber == user.PhoneNumber);

            if (egnToLookFor != null)
            {
                throw new ArgumentException("EGN is already in use");
            }
            if (phoneToLookFor != null)
            {
                throw new ArgumentException("Phone Nubmer is already in use");
            }
            
        }
    }
}
