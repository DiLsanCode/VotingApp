using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VotingApp.Business.Models;
using VotingApp.Data.Models;

namespace VotingApp.Business.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<ViewParticipantInParty>> GetAllListOfParticipant();
        Task<ViewParticipantInParty> GetParticipantById(int id);
        Task Vote(int id, int userId);
    }
}
