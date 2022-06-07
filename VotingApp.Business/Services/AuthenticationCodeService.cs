using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VotingApp.Business.Interfaces;

namespace VotingApp.Business.Services
{
    public class AuthenticationCodeService : IAuthenticationCodeService
    {
        public string CreateAuthenticationCode()
        {
            Random rnd = new Random();
            const string chars = "AaBbCcDdEeFfGgHhIiJjKkkLlMmNnOoPpQqRrSsTtUuVvWwXxYyZz0123456789";
            return new string(Enumerable.Repeat(chars, 8)
                .Select(s => s[rnd.Next(s.Length)]).ToArray());
        }
    }
}
