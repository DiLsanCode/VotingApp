using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VotingApp.Business.Interfaces;

namespace VotingApp.Business.Services
{
    public class HashingService : IHashingService
    {
        private readonly IConfiguration _config;

        public HashingService(IConfiguration config)
        {
            _config = config;
        }

        public string CustomHashing(string str)
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
