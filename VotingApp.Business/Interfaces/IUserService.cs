using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VotingApp.Business.Models;

namespace VotingApp.Business.Interfaces
{
    public interface IUserService
    {
        public Task<string> CreateNewUser(RegisterModel user);
    }
}
